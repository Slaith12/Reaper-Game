using Reaper.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Reaper.Movement;
using Reaper.Combat;

namespace Reaper.Controller
{
    [RequireComponent(typeof(CombatTarget))]
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private PlayerInputs input;
        public static PlayerController player { get; private set; }

        [SerializeField] float maxSpeed;
        [SerializeField] float knockbackStrength = 20f;
        private Vector2 facing;
        private float attackCooldown;

        void Awake()
        {
            mover = GetComponent<Mover>();
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

            input.Player.Attack.performed += Attack;
            input.Player.Attack.Enable();
        }

        private void OnDisable()
        {
            input.Player.Move.Disable();
            input.Player.LookStick.Disable();
            input.Player.LookMouse.Disable();
            input.Player.Attack.Disable();
        }

        private void Damage(Collider2D collision)
        {
            collision.GetComponent<CombatTarget>()?.Damage(3, facing * knockbackStrength, 0.2f);
        }

        #region Input Registering

        private void ChangeMoveDirection(InputAction.CallbackContext obj)
        {
            Vector2 inputSpeed = obj.ReadValue<Vector2>();
            if (inputSpeed.magnitude > 1)
                inputSpeed.Normalize();
            mover.targetSpeed = inputSpeed * maxSpeed;
        }

        private void StickLookDirection(InputAction.CallbackContext obj)
        {
            Vector2 stickPos = obj.ReadValue<Vector2>();
            facing = stickPos.normalized;
        }

        private void MouseLookDirection(InputAction.CallbackContext obj)
        {
            Vector2 mousePos = obj.ReadValue<Vector2>();
            Vector2 relativePos = mousePos - new Vector2(Screen.width / 2, Screen.height / 2);
            facing = relativePos.normalized;
        }

        private void Attack(InputAction.CallbackContext obj)
        {
            MeleeHit.Create(0.25f, Damage, facing, new Vector2(1, 1), new List<string> { "Soul" }, transform, facing.ToAngle());
        }

        #endregion

    }
}
