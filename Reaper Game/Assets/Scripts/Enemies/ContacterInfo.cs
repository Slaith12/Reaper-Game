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
        public float knockbackStrength = 15;
        public float staggerDuration = 0.2f;
        
        protected override int EXTRACOMPS => base.EXTRACOMPS + 1; //DamageObject (contact damage)
        protected DamageObject GetContact(Soul soul) => (DamageObject)soul.extraComponents[base.EXTRACOMPS];

        public override void InitState(Soul soul)
        {
            base.InitState(soul);
            GetContact(soul).OnHit += collision => Damage(soul, collision);
        }

        protected override void Morph(Soul soul)
        {
            base.Morph(soul);
            GetContact(soul).gameObject.SetActive(true);
        }

        protected override void Demorph(Soul soul)
        {
            base.Demorph(soul);
            GetContact(soul).gameObject.SetActive(false);
        }

        private void Damage(Soul soul, Collider2D collision)
        {
            if (soul.state == STATE_UNMORPHED)
                return;
            collision.GetComponent<CombatTarget>()?.Damage(damage, (collision.transform.position - soul.transform.position).normalized * knockbackStrength, staggerDuration);
        }
    }
}