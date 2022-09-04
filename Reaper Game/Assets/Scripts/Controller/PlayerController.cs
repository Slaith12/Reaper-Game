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
        private Vector2 facing;
        public List<Weapon> weapons;
        private Weapon currentWeapon;

        [SerializeField] GameObject shopIndicator;
        public int atShop { get { return shopID; } set
            {
                shopID = value;
                shopIndicator.SetActive(shopID >= 0);
            }
        }
        private int shopID;

        void Awake()
        {
            mover = GetComponent<Mover>();
            input = new PlayerInputs();
            player = this;
        }

        private void Start()
        {
            facing = Vector2.up;
            if (weapons == null)
            {
                weapons = new List<Weapon>();
                weapons.AddRange(GetComponentsInChildren<Weapon>(true));
            }
            foreach (Weapon weap in weapons)
            {
                weap?.Disable();
            }
            if (weapons.Count > 0)
                SwapWeapon(0);
            atShop = -1;
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

            input.Player.Weapon1.performed += _ => SwapWeapon(0);
            input.Player.Weapon1.Enable();
            input.Player.Weapon2.performed += _ => SwapWeapon(1);
            input.Player.Weapon2.Enable();
            input.Player.Weapon3.performed += _ => SwapWeapon(2);
            input.Player.Weapon3.Enable();
            input.Player.Weapon4.performed += _ => SwapWeapon(3);
            input.Player.Weapon4.Enable();

            input.Player.EnterShop.performed += EnterShop;
            input.Player.EnterShop.Enable();
        }

        private void OnDisable()
        {
            input.Player.Move.Disable();
            input.Player.LookStick.Disable();
            input.Player.LookMouse.Disable();
            input.Player.Attack.Disable();
            input.Player.Weapon1.Disable();
            input.Player.Weapon2.Disable();
            input.Player.Weapon3.Disable();
            input.Player.Weapon4.Disable();
            input.Player.EnterShop.Disable();
        }

        private void Update()
        {
            if (currentWeapon == null)
                return;
            currentWeapon.transform.position = (Vector2)transform.position + facing;
            currentWeapon.transform.eulerAngles = new Vector3(0, 0, facing.ToAngle());
        }

        private void SwapWeapon(int index)
        {
            if (weapons.Count <= index)
                return;
            currentWeapon?.Disable();
            currentWeapon = weapons[index];
            currentWeapon?.Enable();
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
            currentWeapon?.Attack(facing);
        }

        private void EnterShop(InputAction.CallbackContext obj)
        {
            if(shopID != -1)
            {
                shopIndicator.SetActive(false);
                ShopManager.instance.OpenShop(shopID);
            }
        }

        #endregion

    }
}
