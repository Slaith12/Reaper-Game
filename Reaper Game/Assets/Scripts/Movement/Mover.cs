using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {
        private class SpeedModifier
        {
            public SpeedModifier(float multiplier, float duration, string type = "None")
            {
                this.multiplier = multiplier;
                this.duration = duration;
                this.type = type;
            }

            public readonly string type;
            public readonly float multiplier;
            public float duration;
        }
        private new Rigidbody2D rigidbody;

        public float acceleration;
        [Min(-0.9f)]
        public float knockbackRes;
        public bool partialKnockback;
        [HideInInspector] public Vector2 targetSpeed;
        public Vector2 effectiveSpeed { get => speedMultiplier == 0 ? Vector2.zero : actualSpeed / speedMultiplier; private set => actualSpeed = value * speedMultiplier; }
        public Vector2 actualSpeed { get => rigidbody.velocity; set => rigidbody.velocity = value; }
        private List<SpeedModifier> speedModifiers;
        public float speedMultiplier { 
            get 
            { 
                float num = 1;
                foreach (SpeedModifier modifier in speedModifiers)
                    num *= modifier.multiplier;
                return num;
            } }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            speedModifiers = new List<SpeedModifier>();
        }

        private void FixedUpdate()
        {
            UpdateModifiers();
            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            if (effectiveSpeed == targetSpeed)
                return;
            float accelSpeed = acceleration * Time.fixedDeltaTime;
            Vector2 accelDirection = targetSpeed - effectiveSpeed;
            if (accelDirection.magnitude <= accelSpeed)
            {
                effectiveSpeed = targetSpeed;
                return;
            }
            effectiveSpeed += accelDirection.normalized * accelSpeed;
        }

        private void UpdateModifiers()
        {
            for (int i = speedModifiers.Count - 1; i >= 0; i--)
            {
                speedModifiers[i].duration -= Time.fixedDeltaTime;
                if (speedModifiers[i].duration <= 0)
                    speedModifiers.RemoveAt(i);
            }
        }

        public bool HasModifierType(string type)
        {
            foreach (SpeedModifier modifier in speedModifiers)
                if (modifier.type == type)
                    return true;
            return false;
        }

        public void Knockback(Vector2 strength, float staggerDuration, float staggerStrength, string staggerType = "Stagger")
        {
            if (partialKnockback)
                actualSpeed += strength / (1 + knockbackRes);
            else
                actualSpeed = strength / (1 + knockbackRes);
            speedModifiers.Add(new SpeedModifier(1 - staggerStrength, staggerDuration, staggerType));
        }

        public void Stun(float duration, bool immediateStop = false, string type = "Stun")
        {
            if (immediateStop)
                actualSpeed = Vector2.zero;
            speedModifiers.Add(new SpeedModifier(0, duration, type));
        }
    }
}
