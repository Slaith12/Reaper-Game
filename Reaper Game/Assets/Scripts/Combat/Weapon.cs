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

        public virtual void PrimaryFireDown(WeaponUser user, Vector2 facing) { }
        public virtual void PrimaryFireHold(WeaponUser user, Vector2 facing) { }
        public virtual void PrimaryFireUp(WeaponUser user, Vector2 facing) { }

        public virtual void SecondaryFireDown(WeaponUser user, Vector2 facing) { }
        public virtual void SecondaryFireHold(WeaponUser user, Vector2 facing) { }
        public virtual void SecondaryFireUp(WeaponUser user, Vector2 facing) { }
    }
}
