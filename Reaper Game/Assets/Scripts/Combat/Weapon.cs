using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public abstract class Weapon : ScriptableObject
    {
        public Sprite sprite;

        public virtual void PrimaryFireDown(WeaponUser user, Vector2 facing) { }
        public virtual void PrimaryFireHold(WeaponUser user, Vector2 facing) { }
        public virtual void PrimaryFireUp(WeaponUser user, Vector2 facing) { }

        public virtual void SecondaryFireDown(WeaponUser user, Vector2 facing) { }
        public virtual void SecondaryFireHold(WeaponUser user, Vector2 facing) { }
        public virtual void SecondaryFireUp(WeaponUser user, Vector2 facing) { }
    }
}
