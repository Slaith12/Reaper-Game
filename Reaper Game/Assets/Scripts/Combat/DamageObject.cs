using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageObject : MonoBehaviour
    {
        public delegate void DamageHandler(Collider2D collision);

        public List<string> targets;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Damage Zone");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targets != null && !targets.Contains(collision.tag))
            {
                return;
            }
            OnHit?.Invoke(collision);
        }

        public event DamageHandler OnHit;
    }
}