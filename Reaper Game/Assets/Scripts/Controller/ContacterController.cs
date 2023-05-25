using Reaper.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContacterController : EnemyController
{
    protected new ContacterInfo attributes => (ContacterInfo)base.attributes;

    protected override void AttackBehavior()
    {
        base.AttackBehavior();
        if (Vector2.Distance(player.transform.position, transform.position) <= attributes.swingRange)
            weaponUser.StartPrimaryAttack();
    }
}
