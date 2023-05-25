using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Data
{
    [CreateAssetMenu(menuName = "Attributes/Player")]
    public class PlayerAttributes : ScriptableObject, MoverAttributes, SoftColliderAttributes
    {
        [Header("Movement")]
        [Min(0)]
        public float moveSpeed = 8;
        [Min(0)]
        public float acceleration = 60;
        [Min(0)]
        [Tooltip("How much higher deceleration is than acceleration. This is not affected by speed modifiers.")]
        public float friction = 30;

        [Header("Collisions")]
        [Min(0.1f)]
        public float knockbackMultiplier = 1;
        public bool partialKnockback = false;
        [Min(0)]
        [Tooltip("When occupying the same space as other characters, how much are the other characters pushed away?")]
        public float crowdingPushForce = 10;

        float MoverAttributes.acceleration => acceleration;
        float MoverAttributes.friction => friction;
        float MoverAttributes.knockbackMult => knockbackMultiplier;
        bool MoverAttributes.partialKnockback => partialKnockback;
        float SoftColliderAttributes.pushForce => crowdingPushForce;
    }
}