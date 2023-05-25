using Reaper.Data;
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

        private MoverAttributes attributes;
        [HideInInspector] public Vector2 targetSpeed;
        public Vector2 effectiveSpeed { get => speedMultiplier == 0 ? Vector2.zero : actualSpeed / speedMultiplier; private set { if (speedMultiplier != 0) actualSpeed = value * speedMultiplier; } }
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

        private void Start()
        {
            AttributeContainer container = GetComponent<AttributeContainer>();
            GetAttributes(container);
            container.OnAttributesChange += delegate { GetAttributes(container); };
        }

        private void GetAttributes(AttributeContainer container)
        {
            attributes = container.GetAttributes<MoverAttributes>();
            if(attributes == null)
            {
                Debug.LogError($"Object {gameObject.name}'s attributes do not include attributes for the mover component. Using default attributes.");
                attributes = BasicAttributes.GetDefaultAttributes<MoverAttributes>();
            }
        }

        private void FixedUpdate()
        {
            UpdateModifiers();
            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            Vector2 accelDirection = targetSpeed - effectiveSpeed;
            if (targetSpeed != effectiveSpeed)
            {
                float accelSpeed = attributes.acceleration * Time.fixedDeltaTime;
                if (accelDirection.magnitude <= accelSpeed)
                {
                    effectiveSpeed = targetSpeed;
                    return;
                }
                effectiveSpeed += accelDirection.normalized * accelSpeed;
            }

            if(speedMultiplier == 0)
            {
                //speed should drift towards 0
                accelDirection = -actualSpeed;
            }

            Vector2 velDirection = actualSpeed.normalized;
            float frictionMult = Vector2.Dot(velDirection, accelDirection.normalized);
            if(frictionMult < 0)
            {
                actualSpeed += attributes.friction * frictionMult * Time.fixedDeltaTime * velDirection;
            }
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
            if (attributes.partialKnockback)
                actualSpeed += strength * attributes.knockbackMult;
            else
                actualSpeed = strength * attributes.knockbackMult;
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
