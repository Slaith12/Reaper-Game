using Reaper.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public class CombatTarget : MonoBehaviour
    {
        public delegate void DeathHandler();

        [HideInInspector] public int health;
        [HideInInspector] public bool invuln;

        private Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        public void Damage(int damage, Vector2 knockback, float stagger)
        {
            if (invuln)
                return;
            health -= damage;
            if (health <= 0)
                OnDeath?.Invoke();
            mover?.Knockback(knockback, stagger);
        }

        public event DeathHandler OnDeath;
    }
}
