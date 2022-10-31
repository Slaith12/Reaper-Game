using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Controller
{
    //public class Bull : Soul
    //{
    //    [SerializeField] float recoilStrength = 10f;
    //    [SerializeField] float chargeTime = 0.75f;
    //    private float chargeTimer;

    //    protected override void Start()
    //    {
    //        base.Start();
    //        contact.OnHit += delegate { mover.Knockback((transform.position - player.transform.position).normalized * recoilStrength, 0.75f); };
    //        chargeTimer = chargeTime;
    //    }

    //    protected override void Patrol()
    //    {
    //        base.Patrol();
    //        chargeTimer = chargeTime;
    //    }

    //    protected override void Chase()
    //    {
    //        if(chargeTimer > 0)
    //        {
    //            chargeTimer -= Time.deltaTime;
    //            mover.targetSpeed = Vector2.zero;
    //            return;
    //        }
    //        if(Vector2.Angle(mover.currentSpeed, player.transform.position-transform.position) >= 90)
    //        {
    //            mover.targetSpeed = Vector2.zero;
    //            if(mover.currentSpeed.magnitude < 3)
    //            {
    //                chargeTimer = chargeTime;
    //            }
    //            mover.partialKnockback = false;
    //            return;
    //        }
    //        base.Chase();
    //        mover.partialKnockback = true;
    //        memoryTimer = 5;
    //    }
    //}
}
