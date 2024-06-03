using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Enemy
{
    //TODO: Improve logic for stopping charge
    //TODO: Let bull push other souls out of the way
    //TODO: Add cooldown state?
    //TODO: Update bull to actually use new systems

    [CreateAssetMenu(fileName = "New Bull", menuName = "Enemies/Bull")]
    public class BullInfo : EnemyInfo
    {
        //public float windUpTime = 1;

        //protected override int EXTRATIMERS => base.EXTRATIMERS + 1;
        //protected int WINDUPINDEX => base.EXTRATIMERS;

        //public override void InitState(EnemyController soul)
        //{
        //    base.InitState(soul);
        //    soul.extraTimers.Add(0);
        //}

        //protected override int NUM_STATES => base.NUM_STATES + 1;
        //public int STATE_WINDUP => base.NUM_STATES;

        //protected override List<StateInfo> states
        //{
        //    get
        //    {
        //        List<StateInfo> states = base.states;
        //        states.Add(new StateInfo(WindupCheck, WindupBehavior));
        //        return states;
        //    }
        //}

        //protected override void Demorph(EnemyController soul)
        //{
        //    base.Demorph(soul);
        //    //soul.mover.partialKnockback = false;
        //}

        //protected override void StartAttack(EnemyController soul)
        //{
        //    StartWindup(soul);
        //}

        //protected virtual void StartWindup(EnemyController soul)
        //{
        //    soul.state = STATE_WINDUP;
        //    soul.memoryTimer = memoryTime;
        //    soul.extraTimers[WINDUPINDEX] = windUpTime;
        //    soul.mover.targetSpeed = Vector2.zero;
        //    //soul.mover.partialKnockback = false;
        //}

        //protected virtual void StartCharge(EnemyController soul)
        //{
        //    soul.state = STATE_ATTACK;
        //    //soul.mover.partialKnockback = true;
        //}

        //protected override void AttackCheck(EnemyController soul)
        //{
        //    Vector2 diff = player.position - soul.transform.position;
        //    if (soul.mover.effectiveSpeed.magnitude > 0.5f && Vector2.Dot(diff, soul.mover.effectiveSpeed) < 0)
        //        StartWindup(soul);
        //}

        //protected override void AttackBehavior(EnemyController soul)
        //{
        //    soul.mover.targetSpeed = (player.transform.position - soul.transform.position).normalized * soul.attributes.chaseSpeed;
        //}

        //protected virtual void WindupCheck(EnemyController soul)
        //{
        //    if (soul.extraTimers[WINDUPINDEX] < 0)
        //        StartCharge(soul);
        //    else if (soul.memoryTimer < 0)
        //        EndAttack(soul);
        //}

        //protected virtual void WindupBehavior(EnemyController soul)
        //{
        //    if(Vector2.Distance(player.transform.position, soul.transform.position) <= chaseDistance)
        //    {
        //        soul.extraTimers[WINDUPINDEX] -= Time.deltaTime;
        //        soul.memoryTimer = memoryTime;
        //    }
        //    else
        //    {
        //        soul.extraTimers[WINDUPINDEX] += Time.deltaTime;
        //        if(soul.extraTimers[WINDUPINDEX] > windUpTime)
        //        {
        //            soul.memoryTimer -= soul.extraTimers[WINDUPINDEX] - windUpTime;
        //            soul.extraTimers[WINDUPINDEX] = windUpTime;
        //        }
        //    }
        //}
    }
}