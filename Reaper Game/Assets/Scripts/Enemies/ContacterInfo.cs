using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Movement;
using Reaper.Combat;

namespace Reaper.Enemy
{
    [CreateAssetMenu(fileName = "New Contacter", menuName = "Enemies/Contacter")]
    public class ContacterInfo : EnemyInfo
    {
        public float swingRange = 2;

        protected override void AttackBehavior(Soul soul)
        {
            base.AttackBehavior(soul);
            if (Vector2.Distance(player.transform.position, soul.transform.position) <= swingRange)
                soul.weaponUser.StartPrimaryAttack();
        }
    }
}