using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Enemy
{
    public abstract class EnemyInfo : ScriptableObject
    {
        public new string name;
        public Sprite sprite;
        public int maxHealth = 5;
        public float acceleration = 80;
        public float patrolSpeed = 5;
        public float attackSpeed = 5;
        public int damage = 1;
        public float knockbackRes = 0;
        public float sightDistance = 7;
        public float chaseDistance = 10;
        public float memoryTime = 5;
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

        public virtual void InitState(Soul soul)
        {
            soul.combatTarget.OnDeath += delegate { Demorph(soul); };
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
        public const int STATE_UNMORPHED = 0;
        public const int STATE_PATROL = 1;
        public const int STATE_ATTACK = 2;
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
            soul.mover.targetSpeed = new Vector2(0, -soul.behavior.patrolSpeed);
        }

        protected virtual void AttackBehavior(Soul soul)
        {
            soul.mover.targetSpeed = (player.transform.position - soul.transform.position).normalized * soul.behavior.attackSpeed;
            soul.memoryTimer -= Time.deltaTime;
            if ((player.transform.position - soul.transform.position).magnitude <= chaseDistance)
                soul.memoryTimer = memoryTime;
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
            if ((player.position - soul.transform.position).magnitude <= soul.behavior.sightDistance)
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
            soul.memoryTimer = soul.behavior.memoryTime;
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
            soul.combatTarget.health = soul.behavior.maxHealth;
            soul.combatTarget.invuln = false;
        }

        //Any -> Unmorphed
        protected virtual void Demorph(Soul soul)
        {
            soul.state = STATE_UNMORPHED;
            soul.combatTarget.invuln = true;
            soul.morphTimer = 5;
            soul.mover.targetSpeed = Vector2.zero;
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