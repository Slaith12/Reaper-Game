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

        public static MeleeHit Create(float duration, Vector2 offset, Vector2 size, List<string> targets, Transform parent = null, float rotation = 0)
        {
            Transform newObject = new GameObject("Melee", typeof(BoxCollider2D), typeof(MeleeHit)).transform;
            newObject.parent = parent;
            newObject.position = offset;
            if(newObject.parent != null)
            {
                newObject.position += newObject.parent.position;
            }
            newObject.eulerAngles = new Vector3(0, 0, rotation);
            newObject.localScale = size;
            MeleeHit melee = newObject.GetComponent<MeleeHit>();
            melee.Init(targets, duration);
            return melee;
        }
    }
}