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
            public SpeedModifier(float multiplier, float duration, string name = "")
            {
                this.multiplier = multiplier;
                this.duration = duration;
                this.name = name;
            }

            public readonly string name;
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
        private float speedMultiplier { 
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
            effectiveSpeed = actualSpeed / speedMultiplier;
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

        public void Knockback(Vector2 strength, float staggerDuration, float staggerStrength)
        {
            if (partialKnockback)
                actualSpeed += strength / (1 + knockbackRes);
            else
                actualSpeed = strength / (1 + knockbackRes);
            speedModifiers.Add(new SpeedModifier(1 - staggerStrength, staggerDuration, name: "Stagger"));
        }

        public void Stun(float duration, bool immediateStop = true)
        {
            if (immediateStop)
                actualSpeed = Vector2.zero;
            speedModifiers.Add(new SpeedModifier(0, duration, name: "Stun"));
        }
    }
}
