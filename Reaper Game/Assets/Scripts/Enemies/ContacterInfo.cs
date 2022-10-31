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
        public float knockbackStrength;
        public float staggerDuration;

        public override void InitState(Soul soul)
        {
            base.InitState(soul);
            DamageObject contact = (DamageObject)soul.extraComponents[0];
            contact.OnHit += collision => Damage(soul, collision);
        }

        private void Damage(Soul soul, Collider2D collision)
        {
            if (soul.state == STATE_UNMORPHED)
                return;
            collision.GetComponent<CombatTarget>()?.Damage(damage, (collision.transform.position - soul.transform.position).normalized * knockbackStrength, staggerDuration);
        }
    }
}