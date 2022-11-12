using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class DamageObject : MonoBehaviour
    {
        public List<string> targets;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Damage Zone");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (ValidateHit(collision))
            {
                OnHit?.Invoke(collision);
            }
        }

        protected virtual bool ValidateHit(Collider2D collision)
        {
            return targets == null || targets.Contains(collision.tag);
        }

        public event DamageHandler OnHit;
    }
}