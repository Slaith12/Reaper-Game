using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [CreateAssetMenu(menuName = "Weapons/Restrainer")]
    public class Restrainer : Weapon
    {
        //I'll likely create a ranged weapon class that restrainer derives from and has these variables
        [SerializeField] protected Sprite projectileSprite;
        [SerializeField] protected float fireCooldown = 0.5f;
        [SerializeField] protected float shotSpeed = 2;
        [SerializeField] protected Vector2 shotSize = Vector2.one;
        [SerializeField] protected float shotDuration = 3;
        [SerializeField] protected List<string> targets;

        public override bool primaryHasHold => true;
        public override bool secondaryHasHold => false;
        public override void SecondaryFireDown(WeaponUser user, Vector2 facing)
        {
            Projectile projectile = Projectile.Create(projectileSprite, targets, Projectile.defaultEnviroTargets, facing * shotSpeed, shotDuration);
            projectile.transform.SetTransform(user.transform.position, facing.ToAngle(), shotSize);
            projectile.OnHit += Capture;
            projectile.OnEnvironmentHit += (_, net) => Destroy(net.gameObject); //will replace this with an animation when implemented
            user.SetCooldown(fireCooldown);
        }

        private void Capture(Collider2D collision, DamageObject net)
        {
            Debug.Log($"Capturing {collision.name}");
            Destroy(net.gameObject);
        }
    }
}