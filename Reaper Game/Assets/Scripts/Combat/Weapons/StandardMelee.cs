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
        [SerializeField] protected List<string> targets;

        public override bool primaryHasHold => false;

        public override bool secondaryHasHold => false;

        public override void PrimaryFireDown(WeaponUser user, Vector2 facing)
        {
            Swing(user, facing, new AttackInfo { type = AttackType.Primary });
        }

        protected virtual void Swing(WeaponUser user, Vector2 facing, AttackInfo info)
        {
            MeleeHit melee = MeleeHit.Create(targets, 0.25f);
            melee.transform.parent = user.transform;
            melee.transform.SetTransform(facing * swingDistance, facing.ToAngle(), swingSize);
            melee.OnHit += (c, d) => Damage(c, d, facing);
            user.SetCooldown(attackCooldown);
            InvokeAttack(info);
        }

        protected virtual void Damage(Collider2D collision, DamageObject damager, Vector2 facing)
        {
            collision.GetComponent<CombatTarget>().Damage(damage, facing * knockbackStrength, staggerLength);
        }
    }
}
