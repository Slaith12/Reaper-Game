using Reaper.Data;
using Reaper.Movement;
using Reaper.Combat;
using Reaper.Messaging;
using System;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Items;

namespace Reaper.Enemy
{
    [RequireComponent(typeof(Mover), typeof(WeaponUser))]
    public abstract class EnemyController : MonoBehaviour, MessageHandler, AttributeContainer
    {
        protected static Transform player { get => Player.PlayerController.player.transform; }
        protected const string NET_IDENTIFIER = "Net";

        public EnemyInfo attributes;

        protected Mover mover;
        protected WeaponUser weaponUser;

        protected Vector2 targetLocation;
        protected float memoryTimer;
        protected float morphTimer;
        protected int state;
        protected int health;

        public event Action OnAttributesChange;

        void Awake()
        {
            mover = GetComponent<Mover>();
            weaponUser = GetComponent<WeaponUser>();

            InitMessages();
        }

        protected virtual void Start()
        {
            weaponUser.SwitchWeapon(attributes.mainWeapon);
            Demorph();
        }

        void Update()
        {
            UpdateState();
            DoStateBehavior();
        }

        #region State Definitions

        protected struct StateInfo
        {
            public Action check;
            public Action behavior;

            public StateInfo(Action check, Action behavior)
            {
                this.check = check;
                this.behavior = behavior;
            }
        }
        
        protected const int NUM_STATES = 4;
        protected const int STATE_UNMORPHED = 0;
        protected const int STATE_PATROL = 1;
        protected const int STATE_ATTACK = 2;
        protected const int STATE_IMMOBILIZED = 3;
        protected virtual List<StateInfo> states
        {
            get
            {
                List<StateInfo> list = new List<StateInfo>
                {
                    new StateInfo(UnmorphedCheck, UnmorphedBehavior),
                    new StateInfo(PatrolCheck, PatrolBehavior),
                    new StateInfo(AttackCheck, AttackBehavior),
                    new StateInfo(ImmobileCheck, ImmobileBehavior)
                };
                return list;
            }
        }

        #endregion

        #region State Behaviors

        private void DoStateBehavior()
        {
            states[state].behavior();
        }

        protected virtual void UnmorphedBehavior()
        {
            morphTimer -= Time.deltaTime;
        }

        protected virtual void PatrolBehavior()
        {
            mover.targetSpeed = new Vector2(0, -attributes.patrolSpeed);
            weaponUser.facing = mover.effectiveSpeed.normalized;
        }

        protected virtual void AttackBehavior()
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= attributes.chaseDistance)
            {
                memoryTimer = attributes.memoryTime;
                targetLocation = player.transform.position;
                AttackMovement();
            }
            else
            {
                memoryTimer -= Time.deltaTime;
                SearchMovement();
            }
            weaponUser.facing = (targetLocation - (Vector2)transform.position).normalized;
        }

        protected virtual void AttackMovement()
        {
            float distance = Vector2.Distance(targetLocation, transform.position);
            Vector2 direction = (targetLocation - (Vector2)transform.position).normalized;
            if (distance > attributes.farOptimalRange)
                mover.targetSpeed = direction * attributes.chaseSpeed;
            else if (distance < attributes.closeOptimalRange)
                mover.targetSpeed = -direction * attributes.chaseSpeed;
            else
                mover.targetSpeed = Vector2.zero;
        }

        protected virtual void SearchMovement()
        {
            float distance = Vector2.Distance(targetLocation, transform.position);
            Vector2 direction = (targetLocation - (Vector2)transform.position).normalized;
            if (distance > 1)
                mover.targetSpeed = direction * attributes.chaseSpeed;
            else
                mover.targetSpeed = Vector2.zero;
        }

        protected virtual void ImmobileBehavior()
        {

        }

        #endregion

        #region State Checks

        private void UpdateState()
        {
            states[state].check();
        }

        protected virtual void UnmorphedCheck()
        {
            if (morphTimer <= 0)
                Morph();
        }

        protected virtual void PatrolCheck()
        {
            if (Vector2.Distance(player.position, transform.position) <= attributes.sightDistance)
                StartAttack();
            //death check handled in InitState
        }

        protected virtual void AttackCheck()
        {
            if (memoryTimer <= 0)
            {
                EndAttack();
            }
            //death check handled in InitState
        }

        protected virtual void ImmobileCheck()
        {
            if (!mover.HasModifierType(NET_IDENTIFIER))
            {
                EndImmobilize();
            }
        }

        #endregion

        #region State Changes

        //Patrol -> Attack
        protected virtual void StartAttack()
        {
            state = STATE_ATTACK;
            memoryTimer = attributes.memoryTime;
        }

        //Attack -> Patrol
        protected virtual void EndAttack()
        {
            state = STATE_PATROL;
        }

        //Unmorphed -> Patrol
        protected virtual void Morph()
        {
            state = STATE_PATROL;
            health = attributes.maxHealth;
        }

        //Any -> Unmorphed
        protected virtual void Demorph()
        {
            state = STATE_UNMORPHED;
            morphTimer = 5;
            mover.targetSpeed = Vector2.zero;
        }

        //Non-Unmorphed -> Immobilized
        protected virtual void StartImmobilize()
        {
            state = STATE_IMMOBILIZED;
            mover.Stun(5, type: NET_IDENTIFIER);
        }

        //Immobilized -> Patrol
        protected virtual void EndImmobilize()
        {
            state = STATE_PATROL;
        }

        #endregion

        #region Message Responses

        protected Dictionary<string, Func<bool>> messageValidators;
        protected Dictionary<string, Action<Message>> messageResponses;

        protected virtual void InitMessages()
        {
            messageValidators = new Dictionary<string, Func<bool>>();
            messageResponses = new Dictionary<string, Action<Message>>();

            AddMessageResponse<DamageMessage>(HandleDamage, ValidateDamage);
            AddMessageResponse<NetCaptureMessage>(HandleNetCapture, ValidateNetCapture);
        }

        protected void AddMessageResponse<T>(Action<T> response, Func<bool> validator = null) where T : Message
        {
            string type = typeof(T).ToString();
            messageResponses.Add(type, m => response((T)m));
            if (validator != null)
                messageValidators.Add(type, validator);
        }

        public bool CanRecieveMessage<T>() where T : Message
        {
            string type = typeof(T).ToString();
            if (messageResponses == null || !messageResponses.ContainsKey(type))
                return false;
            bool hasValidator = messageValidators.TryGetValue(type, out Func<bool> validator);
            if (messageValidators == null || !hasValidator) //a missing validator is treated as simply "does this message handler have a response to this message?"
                return true;
            return validator.Invoke();
        }

        public void InvokeMessage<T>(T message) where T : Message
        {
            if (CanRecieveMessage<T>())
            {
                messageResponses.TryGetValue(typeof(T).ToString(), out Action<Message> handler);
                handler.Invoke(message);
            }
        }

        protected virtual bool ValidateDamage()
        {
            return state != STATE_UNMORPHED;
        }

        protected virtual void HandleDamage(DamageMessage message)
        {
            health -= message.damage;
            if (health <= 0)
                Demorph();
            mover.Knockback(message.knockback, message.staggerDuration, message.staggerIntensity);
            message.consumed = true;
        }

        protected virtual bool ValidateNetCapture()
        {
            return state != STATE_IMMOBILIZED;
        }

        protected virtual void HandleNetCapture(NetCaptureMessage message)
        {
            if (state == STATE_UNMORPHED && attributes.captureItem != null)
            {
                Pickup capturedEnemy = Pickup.Create(attributes.captureItem, transform.position, typeof(Rigidbody2D), typeof(Mover), typeof(BasicAttributes));
                capturedEnemy.GetComponent<Rigidbody2D>().AddForce(message.impactForce, ForceMode2D.Impulse);
                capturedEnemy.GetComponent<BasicAttributes>().AddAttributes<MoverAttributes>(new BasicMoverAttributes(acceleration: 0, friction: attributes.captureItem.friction));
                Destroy(gameObject);
            }
            else
            {
                StartImmobilize();
            }
            message.consumed = true;
        }

        #endregion

        public AttributeType GetAttributes<AttributeType>()
        {
            if (typeof(AttributeType).IsAssignableFrom(attributes.GetType()))
            {
                return (AttributeType)(object)attributes;
            }
            return default;
        }

        public void SetAttributes<AttributeType>(AttributeType attributeSet)
        {
            if (typeof(EnemyInfo).IsAssignableFrom(typeof(AttributeType)))
            {
                attributes = (EnemyInfo)(object)attributeSet;
                OnAttributesChange?.Invoke();
            }
        }
    }
}
