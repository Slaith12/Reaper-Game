using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Messaging
{
    public class DamageMessage : Message
    {

        public DamageMessage(int damage, Vector2 knockback, float staggerDuration = 0, float staggerIntensity = 0.5f)
        {
            this.damage = damage;
            this.knockback = knockback;
            this.staggerDuration = staggerDuration;
            this.staggerIntensity = staggerIntensity;
        }
        public readonly int damage;
        public readonly Vector2 knockback;
        public readonly float staggerDuration;
        public readonly float staggerIntensity;
    }
}