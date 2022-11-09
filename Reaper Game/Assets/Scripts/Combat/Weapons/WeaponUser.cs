using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public class WeaponUser : MonoBehaviour
    {
        public Weapon weapon { get; private set; }

        [HideInInspector] public Vector2 facing;

        private bool primaryFiring;
        private bool secondaryFiring;
        private float primaryCooldown;
        private float secondaryCooldown;

        void Start()
        {
            facing = Vector2.up;
            primaryCooldown = 1;
            secondaryCooldown = 1;
            primaryFiring = false;
            secondaryFiring = false;
        }
        
        void Update()
        {
            if (primaryCooldown > 0)
                primaryCooldown -= Time.deltaTime;
            if (secondaryCooldown > 0)
                secondaryCooldown -= Time.deltaTime;
            if (primaryFiring)
                weapon.PrimaryFireHold(this, facing);
            if (secondaryFiring)
                weapon.SecondaryFireHold(this, facing);
        }

        public void SetCooldown(float duration, AttackType type = AttackType.Both)
        {
            if (type == AttackType.Primary || type == AttackType.Both)
                primaryCooldown = duration;
            if (type == AttackType.Secondary || type == AttackType.Both)
                secondaryCooldown = duration;
        }

        public void SwitchWeapon(Weapon weapon)
        {
            if (primaryFiring)
            {
                EndPrimaryAttack();
            }
            if (secondaryFiring)
            {
                EndSecondaryAttack();
            }
            this.weapon = weapon;
            this.weapon.Equip();
        }

        #region Attack Events

        public void StartPrimaryAttack()
        {
            if (!weapon || primaryCooldown > 0)
                return;
            weapon.PrimaryFireDown(this, facing);
            if(weapon.primaryHasHold)
                primaryFiring = true;
        }

        public void EndPrimaryAttack()
        {
            if (!weapon || !primaryFiring)
                return;
            weapon.PrimaryFireUp(this, facing);
            primaryFiring = false;
        }

        public void StartSecondaryAttack()
        {
            if (!weapon || secondaryCooldown > 0)
                return;
            weapon.SecondaryFireDown(this, facing);
            if(weapon.secondaryHasHold)
                secondaryFiring = true;
        }

        public void EndSecondaryAttack()
        {
            if (!weapon || !secondaryFiring)
                return;
            weapon.SecondaryFireUp(this, facing);
            secondaryFiring = false;
        }

        #endregion

    }
}