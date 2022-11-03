using Reaper.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Reaper.Movement;
using Reaper.Combat;
using Reaper.Shops;

namespace Reaper.Player
{
    [RequireComponent(typeof(CombatTarget), typeof(Mover), typeof(WeaponUser))]
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private WeaponUser weapons;
        private PlayerInputs input;
        public static PlayerController player { get; private set; }

        [SerializeField] float maxSpeed;

        [SerializeField] GameObject shopIndicator;

        void Awake()
        {
            mover = GetComponent<Mover>();
            weapons = GetComponent<WeaponUser>();
            input = new PlayerInputs();
            player = this;
        }

        private void Start()
        {
            ShopManager.instance.OnApproachShop += delegate { shopIndicator.SetActive(true); };
            ShopManager.instance.OnLeaveShop += delegate { shopIndicator.SetActive(false); };
        }

        #region Input System Handling

        private void OnEnable()
        {
            input.Player.Enable();

            input.Player.Move.performed += ChangeMoveDirection;
            input.Player.Move.canceled += ChangeMoveDirection;

            //stick look and mouse look need to be seperated since they use different logic and I can't detect which control was used otherwise
            //I could guess based on the value given (Stick would have values in the range (-1, 1)), but that would cause issues with the mouse look if it goes in the corner
            input.Player.LookStick.performed += StickLookDirection;
            input.Player.LookMouse.performed += MouseLookDirection;

            input.Player.Attack.performed += delegate { weapons.StartPrimaryAttack(); };
            input.Player.Attack.canceled += delegate { weapons.EndPrimaryAttack(); };

            input.Player.Weapon1.performed += delegate { weapons.SwitchWeapon(0); };
            input.Player.Weapon2.performed += delegate { weapons.SwitchWeapon(1); };
            input.Player.Weapon3.performed += delegate { weapons.SwitchWeapon(2); };
            input.Player.Weapon4.performed += delegate { weapons.SwitchWeapon(3); };

            input.Player.EnterShop.performed += EnterShop;
        }

        private void OnDisable()
        {
            input.Player.Disable();
        }

        #endregion

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
            weapons.facing = stickPos.normalized;
        }

        private void MouseLookDirection(InputAction.CallbackContext obj)
        {
            Vector2 mousePos = obj.ReadValue<Vector2>();
            Vector2 relativePos = mousePos - new Vector2(Screen.width / 2, Screen.height / 2);
            weapons.facing = relativePos.normalized;
        }

        private void EnterShop(InputAction.CallbackContext obj)
        {
            ShopManager.instance.OpenShop();
        }

        #endregion

    }
}
