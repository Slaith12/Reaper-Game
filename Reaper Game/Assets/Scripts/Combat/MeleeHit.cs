using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public class MeleeHit : DamageObject
    {
        private float duration;
        private List<Collider2D> prevHits;

        public void Init(List<string> targets, float duration)
        {
            this.targets = targets;
            this.duration = duration;
            prevHits = new List<Collider2D>();
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected override bool ValidateHit(Collider2D collision)
        {
            if (!base.ValidateHit(collision))
                return false;
            if (prevHits.Contains(collision))
                return false;
            prevHits.Add(collision);
            return true;
        }

        public static MeleeHit Create(List<string> targets, float duration)
        {
            MeleeHit melee = new GameObject("Melee", typeof(BoxCollider2D), typeof(MeleeHit)).GetComponent<MeleeHit>();
            melee.Init(targets, duration);
            return melee;
        }
    }
}