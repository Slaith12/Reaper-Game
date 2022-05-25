using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public class Scythe : Weapon
    {
        [SerializeField] int damage = 3;
        [SerializeField] float knockbackStrength = 15;
        [SerializeField] float attackInterval = 0.5f;
        private float cooldown;
        
        void Update()
        {
            if (cooldown > 0)
                cooldown -= Time.deltaTime;
        }

        public override void Attack(Vector2 facing)
        {
            if (cooldown > 0)
                return;
            MeleeHit.Create(0.25f, c => Damage(c, facing), facing, new Vector2(1, 1), new List<string> { "Soul" }, transform.parent, facing.ToAngle());
            cooldown = attackInterval;
        }

        private void Damage(Collider2D collision, Vector2 facing)
        {
            collision.GetComponent<CombatTarget>()?.Damage(damage, facing * knockbackStrength, 0.2f);
        }
    }
}
