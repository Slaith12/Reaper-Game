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

        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!targets.Contains(collision.tag))
            {
                return;
            }
            OnHit?.Invoke(collision);
            Debug.Log($"Damage Object {name} hit");
        }

        public event DamageHandler OnHit;
    }
}