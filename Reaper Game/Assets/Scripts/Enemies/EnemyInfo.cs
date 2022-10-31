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
        public float attackSpeed = 5;
        public int damage = 1;
        public float sightDistance = 10;
        public float memoryTime = 5;
        public float patrolSpeed = 5;
        public GameObject templateObject;

        protected delegate void SoulAction(Soul soul);
        public const int STATE_UNMORPHED = 0;
        public const int STATE_PATROL = 1;
        public const int STATE_ATTACK = 2;
        protected static Transform player { get => Player.PlayerController.player.transform; }

        public virtual void InitState(Soul soul)
        {
            soul.morphTimer = 5;
            soul.state = STATE_UNMORPHED;
            soul.combatTarget.OnDeath += delegate { Demorph(soul); };
            Demorph(soul);
        }

        public void Update(Soul soul)
        {
            Debug.Log($"Update {soul.state}");
            UpdateState(soul);
            DoStateBehavior(soul);
        }

        #region State Behaviors

        private void DoStateBehavior(Soul soul)
        {
            GetStateBehaviors()[soul.state](soul);
        }

        protected virtual SoulAction[] GetStateBehaviors()
        {
            return new SoulAction[] { Unmorphed, Patrol, Attack };
        }

        protected virtual void Unmorphed(Soul soul)
        {
            soul.morphTimer -= Time.deltaTime;
        }

        protected virtual void Patrol(Soul soul)
        {
            soul.mover.targetSpeed = new Vector2(0, -soul.behavior.patrolSpeed);
        }

        protected virtual void Attack(Soul soul)
        {
            soul.mover.targetSpeed = (player.transform.position - soul.transform.position).normalized * soul.behavior.attackSpeed;
            soul.memoryTimer -= Time.deltaTime;
            if ((player.transform.position - soul.transform.position).magnitude <= sightDistance)
                soul.memoryTimer = memoryTime;
        }

        #endregion

        #region State Checks

        private void UpdateState(Soul soul)
        {
            GetStateChecks()[soul.state](soul);
        }

        protected virtual SoulAction[] GetStateChecks()
        {
            return new SoulAction[] { UnmorphedCheck, PatrolCheck, AttackCheck };
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
            return soul;
        }
    }
}