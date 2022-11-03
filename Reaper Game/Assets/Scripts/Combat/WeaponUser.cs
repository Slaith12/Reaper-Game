using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public class WeaponUser : MonoBehaviour
    {
        [SerializeField] List<Weapon> weapons;
        Weapon currentWeapon;

        [HideInInspector] public Vector2 facing;

        private bool primaryFiring;
        private bool secondaryFiring;
        private float primaryCooldown;
        private float secondaryCooldown;
        
        public enum AttackType { Primary, Secondary, Both }

        void Start()
        {
            facing = Vector2.up;
            SwitchWeapon(0);
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
                currentWeapon.PrimaryFireHold(this, facing);
            if (secondaryFiring)
                currentWeapon.SecondaryFireHold(this, facing);
        }

        public void SetCooldown(float duration, AttackType type = AttackType.Both)
        {
            if (type == AttackType.Primary || type == AttackType.Both)
                primaryCooldown = duration;
            if (type == AttackType.Secondary || type == AttackType.Both)
                secondaryCooldown = duration;
        }

        #region Weapon Swapping

        public int AddWeapon(Weapon weapon)
        {
            int index = weapons.FindIndex(w => w == weapon);
            if(index < 0)
            {
                index = weapons.Count;
                weapons.Add(weapon);
            }
            return index;
        }

        public void SwitchWeapon(int index)
        {
            if (weapons == null || weapons.Count <= index)
                return;
            if (primaryFiring)
            {
                currentWeapon.PrimaryFireUp(this, facing);
                primaryFiring = false;
            }
            if(secondaryFiring)
            {
                currentWeapon.SecondaryFireUp(this, facing);
                secondaryFiring = false;
            }
            currentWeapon = weapons[index];
        }

        public void SwitchWeapon(Weapon weapon)
        {
            SwitchWeapon(AddWeapon(weapon));
        }

        #endregion

        #region Attack Events

        public void StartPrimaryAttack()
        {
            if (primaryCooldown > 0)
                return;
            primaryFiring = true;
            currentWeapon.PrimaryFireDown(this, facing);
        }

        public void EndPrimaryAttack()
        {
            if (!primaryFiring)
                return;
            primaryFiring = false;
            currentWeapon.PrimaryFireUp(this, facing);
        }

        public void StartSecondaryAttack()
        {
            if (secondaryCooldown > 0)
                return;
            secondaryFiring = true;
            currentWeapon.SecondaryFireDown(this, facing);
        }

        public void EndSecondaryAttack()
        {
            if (!secondaryFiring)
                return;
            secondaryFiring = false;
            currentWeapon.SecondaryFireUp(this, facing);
        }

        #endregion

    }
}