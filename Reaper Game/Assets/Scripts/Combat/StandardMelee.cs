using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [CreateAssetMenu(menuName = "Weapons/Melee Weapon")]
    public class StandardMelee : Weapon
    {
        [SerializeField] int damage = 3;
        [SerializeField] float attackCooldown = 0.5f;
        [SerializeField] float knockbackStrength = 15;
        [SerializeField] float staggerLength = 0.2f;
        [SerializeField] float swingDistance = 1;
        [SerializeField] Vector2 swingSize = Vector2.one;

        public override void PrimaryFireDown(WeaponUser user, Vector2 facing)
        {
            MeleeHit.Create(0.25f, c => Damage(c, facing), facing*swingDistance, swingSize, new List<string> { "Soul" }, user.transform, facing.ToAngle());
            user.SetCooldown(attackCooldown);
        }

        private void Damage(Collider2D collision, Vector2 facing)
        {
            collision.GetComponent<CombatTarget>()?.Damage(damage, facing * knockbackStrength, staggerLength);
        }
    }
}
