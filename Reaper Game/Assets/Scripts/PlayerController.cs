using Reaper.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reaper.Controller
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] float maxSpeed;
        [SerializeField] float acceleration;
        private Vector2 targetSpeed;
        private Vector2 currentSpeed;
        private float knockbackTimer;

        private float facing;

        private new Rigidbody2D rigidbody;
        private PlayerInputs input;
        public static PlayerController player { get; private set; }

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            input = new PlayerInputs();
            player = this;
        }

        private void OnEnable()
        {
            input.Player.Move.performed += ChangeMoveDirection;
            input.Player.Move.canceled += ChangeMoveDirection;
            input.Player.Move.Enable();

            //stick look and mouse look need to be seperated since they use different logic and I can't detect which control was used otherwise
            //I could guess based on the value given (Stick would have values in the range (-1, 1)), but that would cause issues with the mouse look if it goes in the corner
            input.Player.LookStick.performed += StickLookDirection;
            input.Player.LookStick.Enable();

            input.Player.LookMouse.performed += MouseLookDirection;
            input.Player.LookMouse.Enable();
        }

        private void OnDisable()
        {
            input.Player.Move.Disable();
            input.Player.LookStick.Disable();
            input.Player.LookMouse.Disable();
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
            if(knockbackTimer > 0)
            {
                knockbackTimer -= Time.fixedDeltaTime;
                accelSpeed /= 2;
            }
            Debug.Log(accelSpeed);
            Vector2 accelDirection = targetSpeed - currentSpeed;
            if (accelDirection.magnitude <= accelSpeed)
            {
                currentSpeed = targetSpeed;
                return;
            }
            currentSpeed += accelDirection.normalized * accelSpeed;
        }

        public void Knockback(Vector2 strength, float duration)
        {
            currentSpeed = strength;
            knockbackTimer = duration;
        }

        #region Input Registering

        private void ChangeMoveDirection(InputAction.CallbackContext obj)
        {
            Vector2 inputSpeed = obj.ReadValue<Vector2>();
            if (inputSpeed.magnitude > 1)
                inputSpeed.Normalize();
            targetSpeed = inputSpeed * maxSpeed;
        }

        private void StickLookDirection(InputAction.CallbackContext obj)
        {
            Vector2 stickPos = obj.ReadValue<Vector2>();
            facing = Mathf.Atan(stickPos.x / stickPos.y) * 180 / Mathf.PI;
            if (facing < 0)
                facing += 180;
            if (stickPos.x < 0)
                facing += 180;
        }

        private void MouseLookDirection(InputAction.CallbackContext obj)
        {
            Vector2 mousePos = obj.ReadValue<Vector2>();
            Vector2 relativePos = mousePos - new Vector2(Screen.width / 2, Screen.height / 2);
            facing = Mathf.Atan(relativePos.x / relativePos.y) * 180 / Mathf.PI;
            if (facing < 0)
                facing += 180;
            if (relativePos.x < 0)
                facing += 180;
        }

        #endregion

    }
}
