using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public abstract class Weapon : ScriptableObject
    {
        public Sprite sprite;

        //will likely be used for animations
        public event Action OnEquip;
        public event AttackResponse OnAttack;

        public virtual void Equip()
        {
            OnEquip?.Invoke();
        }

        protected void InvokeAttack(AttackInfo info)
        {
            OnAttack?.Invoke(info);
        }

        /// <summary>
        /// Indicates whether the weapon has any function on primary fire hold or up
        /// </summary>
        public abstract bool primaryHasHold { get; }
        public virtual void PrimaryFireDown(WeaponUser user, Vector2 facing) { }
        public virtual void PrimaryFireHold(WeaponUser user, Vector2 facing) { }
        public virtual void PrimaryFireUp(WeaponUser user, Vector2 facing) { }

        /// <summary>
        /// Indicates whether the weapon has any function on secondary fire hold or up
        /// </summary>
        public abstract bool secondaryHasHold { get; }
        public virtual void SecondaryFireDown(WeaponUser user, Vector2 facing) { }
        public virtual void SecondaryFireHold(WeaponUser user, Vector2 facing) { }
        public virtual void SecondaryFireUp(WeaponUser user, Vector2 facing) { }
    }
}
