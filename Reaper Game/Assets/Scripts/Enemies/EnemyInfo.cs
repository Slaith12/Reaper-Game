using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Combat;
using Reaper.Messaging;
using System;

namespace Reaper.Enemy
{
    public abstract class EnemyInfo : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
        public int maxHealth = 5;
        public float acceleration = 80;
        public float patrolSpeed = 5;
        public float chaseSpeed = 5;
        public float closeOptimalRange = 0.75f;
        public float farOptimalRange = 1.5f;
        public float knockbackRes = 0;
        public float sightDistance = 7;
        public float chaseDistance = 10;
        public float memoryTime = 5;
        public Weapon mainWeapon;
        public GameObject templateObject;

        protected delegate void SoulAction(Soul soul);

        protected static Transform player { get => Player.PlayerController.player.transform; }

        /// <summary>
        /// The number of extra components used by this enemy. If a child enemy uses more, type
        /// <code>protected override int EXTRACOMPS => base.EXTRACOMPS + [extra components];</code>
        /// When accessing an extra component, do it in the style
        /// <code>soul.extraComponents[base.EXTRACOMPS + offset]</code>
        /// </summary>
        protected virtual int EXTRACOMPS => 0;
        /// <summary>
        /// The number of extra timers used by this enemy. If a child enemy uses more, type
        /// <code>protected override int EXTRATIMERS => base.EXTRATIMERS + [extra timers];</code>
        /// When accessing an extra timer, do it in the style
        /// <code>soul.extraTimers[base.EXTRATIMERS + offset]</code>
        /// </summary>
        protected virtual int EXTRATIMERS => 0;

        private void OnEnable()
        {
            InitMessages(); //it would be really nice if this could be done during compililation rather than during run-time, but I don't know how to do that
        }

        public virtual void InitState(Soul soul)
        {
            soul.weaponUser.SwitchWeapon(mainWeapon);
            Demorph(soul);
        }

        public void UpdateSoul(Soul soul)
        {
            UpdateState(soul);
            DoStateBehavior(soul);
        }

        #region State Definitions

        protected struct StateInfo
        {
            public SoulAction stateCheck;
            public SoulAction stateBehavior;

            public StateInfo(SoulAction check, SoulAction behavior)
            {
                stateCheck = check;
                stateBehavior = behavior;
            }
        }
        protected virtual int NUM_STATES => 3;
        public int STATE_UNMORPHED => 0;
        public int STATE_PATROL => 1;
        public int STATE_ATTACK => 2;
        protected virtual List<StateInfo> states
        {
            get
            {
                List<StateInfo> list = new List<StateInfo>();
                list.Add(new StateInfo(UnmorphedCheck, UnmorphedBehavior));
                list.Add(new StateInfo(PatrolCheck, PatrolBehavior));
                list.Add(new StateInfo(AttackCheck, AttackBehavior));
                return list;
            }
        }

        #endregion

        #region State Behaviors

        private void DoStateBehavior(Soul soul)
        {
            states[soul.state].stateBehavior(soul);
        }

        protected virtual void UnmorphedBehavior(Soul soul)
        {
            soul.morphTimer -= Time.deltaTime;
        }

        protected virtual void PatrolBehavior(Soul soul)
        {
            soul.mover.targetSpeed = new Vector2(0, -patrolSpeed);
            soul.weaponUser.facing = soul.mover.effectiveSpeed.normalized;
        }

        protected virtual void AttackBehavior(Soul soul)
        {
            if (Vector2.Distance(player.transform.position, soul.transform.position) <= chaseDistance)
            {
                soul.memoryTimer = memoryTime;
                soul.targetLocation = player.transform.position;
                AttackMovement(soul);
            }
            else
            {
                soul.memoryTimer -= Time.deltaTime;
                SearchMovement(soul);
            }
            soul.weaponUser.facing = (soul.targetLocation - (Vector2)soul.transform.position).normalized;
        }

        protected virtual void AttackMovement(Soul soul)
        {
            float distance = Vector2.Distance(soul.targetLocation, soul.transform.position);
            Vector2 direction = (soul.targetLocation - (Vector2)soul.transform.position).normalized;
            if (distance > farOptimalRange)
                soul.mover.targetSpeed = direction * chaseSpeed;
            else if (distance < closeOptimalRange)
                soul.mover.targetSpeed = -direction * chaseSpeed;
            else
                soul.mover.targetSpeed = Vector2.zero;
        }

        protected virtual void SearchMovement(Soul soul)
        {
            float distance = Vector2.Distance(soul.targetLocation, soul.transform.position);
            Vector2 direction = (soul.targetLocation - (Vector2)soul.transform.position).normalized;
            if (distance > 1)
                soul.mover.targetSpeed = direction * chaseSpeed;
            else
                soul.mover.targetSpeed = Vector2.zero;
        }

        #endregion

        #region State Checks

        private void UpdateState(Soul soul)
        {
            states[soul.state].stateCheck(soul);
        }

        protected virtual void UnmorphedCheck(Soul soul)
        {
            if (soul.morphTimer <= 0)
                Morph(soul);
        }

        protected virtual void PatrolCheck(Soul soul)
        {
            if (Vector2.Distance(player.position, soul.transform.position) <= sightDistance)
                StartAttack(soul);
            //death check handled in InitState
        }

        protected virtual void AttackCheck(Soul soul)
        {
            if (soul.memoryTimer <= 0)
            {
                EndAttack(soul);
            }
            //death check handled in InitState
        }

        #endregion

        #region State Changes

        //Patrol -> Attack
        protected virtual void StartAttack(Soul soul)
        {
            soul.state = STATE_ATTACK;
            soul.memoryTimer = memoryTime;
        }

        //Attack -> Patrol
        protected virtual void EndAttack(Soul soul)
        {
            soul.state = STATE_PATROL;
        }

        //Unmorphed -> Patrol
        protected virtual void Morph(Soul soul)
        {
            soul.state = STATE_PATROL;
            soul.health = maxHealth;
        }

        //Any -> Unmorphed
        protected virtual void Demorph(Soul soul)
        {
            soul.state = STATE_UNMORPHED;
            soul.morphTimer = 5;
            soul.mover.targetSpeed = Vector2.zero;
        }

        #endregion

        #region Message Responses

        protected Dictionary<string, Func<Soul, bool>> messageValidators;
        protected Dictionary<string, Action<Soul, Message>> messageResponses;

        protected virtual void InitMessages()
        {
            messageValidators = new Dictionary<string, Func<Soul, bool>>();
            messageResponses = new Dictionary<string, Action<Soul, Message>>();

            AddMessageResponse<DamageMessage>(HandleDamage, ValidateDamage);

        }

        protected void AddMessageResponse<T>(Action<Soul, T> response, Func<Soul, bool> validator = null) where T : Message
        {
            string type = typeof(T).ToString();
            messageResponses.Add(type, (s, m) => response(s, (T)m));
            if (validator != null)
                messageValidators.Add(type, validator);
        }

        public bool CanRecieveMessage<T>(Soul soul) where T : Message
        {
            string type = typeof(T).ToString();
            if (messageResponses == null || !messageResponses.ContainsKey(type))
                return false;
            bool hasValidator = messageValidators.TryGetValue(type, out Func<Soul, bool> validator);
            if (messageValidators == null || !hasValidator) //a missing validator is treated as simply "does this message handler have a response to this message?"
                return true;
            return validator.Invoke(soul);
        }

        public void InvokeMessage<T>(Soul soul, T message) where T : Message
        {
            if (CanRecieveMessage<T>(soul))
            {
                messageResponses.TryGetValue(typeof(T).ToString(), out Action<Soul, Message> handler);
                handler.Invoke(soul, message);
            }
        }

        protected virtual bool ValidateDamage(Soul soul)
        {
            return soul.state != STATE_UNMORPHED;
        }

        protected virtual void HandleDamage(Soul soul, DamageMessage message)
        {
            soul.health -= message.damage;
            if (soul.health <= 0)
                Demorph(soul);
            soul.mover.Knockback(message.knockback, message.staggerDuration, message.staggerIntensity);
            message.consumed = true;
        }

        #endregion

        public Soul Create(Vector2 position)
        {
            Soul soul = Instantiate(templateObject, position, Quaternion.Euler(0, 0, 0)).GetComponent<Soul>();
            soul.behavior = this;
            soul.GetComponent<SpriteRenderer>().sprite = sprite;
            soul.gameObject.name = name;
            soul.mover.acceleration = acceleration;
            soul.mover.knockbackRes = knockbackRes;
            return soul;
        }
    }
}