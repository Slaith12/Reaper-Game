using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    {
        private new Rigidbody2D rigidbody;

        [SerializeField] float acceleration;
        [Min(-0.9f)]
        [SerializeField] float knockbackRes;
        [HideInInspector] public Vector2 targetSpeed;
        private Vector2 currentSpeed;
        private float staggerTimer;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
            rigidbody.velocity = currentSpeed;
        }

        private void UpdateSpeed()
        {
            if (currentSpeed == targetSpeed)
                return;
            float accelSpeed = acceleration * Time.fixedDeltaTime;
            if (staggerTimer > 0)
            {
                staggerTimer -= Time.fixedDeltaTime;
                accelSpeed /= 2;
            }
            Vector2 accelDirection = targetSpeed - currentSpeed;
            if (accelDirection.magnitude <= accelSpeed)
            {
                currentSpeed = targetSpeed;
                return;
            }
            currentSpeed += accelDirection.normalized * accelSpeed;
        }

        public void Knockback(Vector2 strength, float stagger)
        {
            currentSpeed = strength/(1+knockbackRes);
            staggerTimer = stagger;
        }
    }
}
