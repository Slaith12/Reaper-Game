using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [CreateAssetMenu(menuName = "Weapons/Melee Weapon")]
    public class StandardMelee : Weapon
    {
        [SerializeField] protected int damage = 3;
        [SerializeField] protected float attackCooldown = 0.5f;
        [SerializeField] protected float knockbackStrength = 15;
        [SerializeField] protected float staggerLength = 0.2f;
        [SerializeField] protected float swingDistance = 1;
        [SerializeField] protected Vector2 swingSize = Vector2.one;

        public override void PrimaryFireDown(WeaponUser user, Vector2 facing)
        {
            Swing(user, facing, new AttackInfo { type = AttackType.Primary });
        }

        protected virtual void Swing(WeaponUser user, Vector2 facing, AttackInfo info)
        {
            MeleeHit.Create(0.25f, c => Damage(c, facing), facing * swingDistance, swingSize, new List<string> { "Soul" }, user.transform, facing.ToAngle());
            user.SetCooldown(attackCooldown);
            InvokeAttack(info);
        }

        protected virtual void Damage(Collider2D collision, Vector2 facing)
        {
            collision.GetComponent<CombatTarget>()?.Damage(damage, facing * knockbackStrength, staggerLength);
        }
    }
}
