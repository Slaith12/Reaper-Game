using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Enemy
{
    //TODO:
    //Improve logic for stopping charge
    //Let bull push other souls out of the way
    //Add cooldown state?

    [CreateAssetMenu(fileName = "New Bull", menuName = "Enemies/Bull")]
    public class BullInfo : ContacterInfo
    {
        public float windUpTime = 1;

        protected override int EXTRATIMERS => base.EXTRATIMERS + 1;
        protected int WINDUPINDEX => base.EXTRATIMERS;

        public override void InitState(Soul soul)
        {
            base.InitState(soul);
            soul.extraTimers.Add(0);
        }

        public const int STATE_WINDUP = 3;

        protected override List<StateInfo> states
        {
            get
            {
                List<StateInfo> states = base.states;
                states.Add(new StateInfo(WindupCheck, WindupBehavior));
                return states;
            }
        }

        protected override void Demorph(Soul soul)
        {
            base.Demorph(soul);
            soul.mover.partialKnockback = false;
        }

        protected override void StartAttack(Soul soul)
        {
            StartWindup(soul);
        }

        protected virtual void StartWindup(Soul soul)
        {
            soul.state = STATE_WINDUP;
            soul.memoryTimer = memoryTime;
            soul.extraTimers[WINDUPINDEX] = windUpTime;
            soul.mover.targetSpeed = Vector2.zero;
            soul.mover.partialKnockback = false;
        }

        protected virtual void StartCharge(Soul soul)
        {
            soul.state = STATE_ATTACK;
            soul.mover.partialKnockback = true;
        }

        protected override void AttackCheck(Soul soul)
        {
            Vector2 diff = player.position - soul.transform.position;
            if (soul.mover.currentSpeed.magnitude > 0.5f && Vector2.Dot(diff, soul.mover.currentSpeed) < 0)
                StartWindup(soul);
        }

        protected override void AttackBehavior(Soul soul)
        {
            soul.mover.targetSpeed = (player.transform.position - soul.transform.position).normalized * soul.behavior.attackSpeed;
        }

        protected virtual void WindupCheck(Soul soul)
        {
            if (soul.extraTimers[WINDUPINDEX] < 0)
                StartCharge(soul);
            else if (soul.memoryTimer < 0)
                EndAttack(soul);
        }

        protected virtual void WindupBehavior(Soul soul)
        {
            if((player.transform.position - soul.transform.position).magnitude <= chaseDistance)
            {
                soul.extraTimers[WINDUPINDEX] -= Time.deltaTime;
                soul.memoryTimer = memoryTime;
            }
            else
            {
                soul.extraTimers[WINDUPINDEX] += Time.deltaTime;
                if(soul.extraTimers[WINDUPINDEX] > windUpTime)
                {
                    soul.memoryTimer -= soul.extraTimers[WINDUPINDEX] - windUpTime;
                    soul.extraTimers[WINDUPINDEX] = windUpTime;
                }
            }
        }
    }
}