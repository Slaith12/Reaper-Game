using Reaper.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Reaper.Movement;
using Reaper.Combat;
using Reaper.Shops;
using Reaper.Messaging;

namespace Reaper.Player
{
    [RequireComponent(typeof(Mover), typeof(WeaponUser))]
    public class PlayerController : MonoBehaviour, IMessageHandler
    {
        private Mover mover;
        private WeaponUser weaponUser;
        private PlayerInputs input;
        [SerializeField] List<Weapon> weapons;
        [SerializeField] SpriteRenderer weaponDisplay; //will be replaced when animations are implemented
        public static PlayerController player { get; private set; }

        [SerializeField] float maxSpeed;

        [SerializeField] GameObject shopIndicator; //will be replaced when animations are implemented

        void Awake()
        {
            mover = GetComponent<Mover>();
            weaponUser = GetComponent<WeaponUser>();
            input = new PlayerInputs();
            player = this;
        }

        private void Start()
        {
            ShopManager.instance.OnApproachShop += delegate { shopIndicator.SetActive(true); };
            ShopManager.instance.OnLeaveShop += delegate { shopIndicator.SetActive(false); };
            SetupInput();
            SwapWeapon(0);
            InitMessages();
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }

        private void OnDisable()
        {
            input.Player.Disable();
        }

        private void ChangeFacing(Vector2 direction)
        {
            weaponUser.facing = direction;
            weaponDisplay.transform.position = (Vector2)transform.position + direction;
            weaponDisplay.transform.eulerAngles = new Vector3(0, 0, direction.ToAngle());
        }

        private void SwapWeapon(int weaponIndex)
        {
            if (weapons == null || weapons.Count <= weaponIndex)
                return;
            weaponUser.SwitchWeapon(weapons[weaponIndex]);
            weaponDisplay.sprite = weaponUser.weapon.sprite;
        }

        #region Input Handling

        private void SetupInput()
        {
            input.Player.Move.performed += ChangeMoveDirection;
            input.Player.Move.canceled += ChangeMoveDirection;

            //stick look and mouse look need to be seperated since they use different logic and I can't detect which control was used otherwise
            //I could guess based on the value given (Stick would have values in the range (-1, 1)), but that would cause issues with the mouse look if it goes in the corner
            input.Player.LookStick.performed += StickLookDirection;
            input.Player.LookMouse.performed += MouseLookDirection;

            input.Player.PrimaryAttack.performed += delegate { weaponUser.StartPrimaryAttack(); };
            input.Player.PrimaryAttack.canceled += delegate { weaponUser.EndPrimaryAttack(); };
            input.Player.SecondaryAttack.performed += delegate { weaponUser.StartSecondaryAttack(); };
            input.Player.SecondaryAttack.canceled += delegate { weaponUser.EndSecondaryAttack(); };

            input.Player.Weapon1.performed += delegate { SwapWeapon(0); };
            input.Player.Weapon2.performed += delegate { SwapWeapon(1); };
            input.Player.Weapon3.performed += delegate { SwapWeapon(2); };
            input.Player.Weapon4.performed += delegate { SwapWeapon(3); };

            input.Player.EnterShop.performed += EnterShop;
        }

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
            ChangeFacing(stickPos.normalized);
        }

        private void MouseLookDirection(InputAction.CallbackContext obj)
        {
            Vector2 mousePos = obj.ReadValue<Vector2>();
            Vector2 relativePos = mousePos - new Vector2(Screen.width / 2, Screen.height / 2);
            ChangeFacing(relativePos.normalized);
        }

        private void EnterShop(InputAction.CallbackContext obj)
        {
            ShopManager.instance.OpenShop();
        }

        #endregion

        #region Message Handling

        Dictionary<string, Action<Message>> messageResponses;

        private void InitMessages()
        {
            messageResponses = new Dictionary<string, Action<Message>>();
            messageResponses.Add(typeof(DamageMessage).ToString(), m => HandleDamage((DamageMessage)m));
        }

        public bool CanRecieveMessage<T>() where T : Message
        {
            return messageResponses.ContainsKey(typeof(T).ToString());
        }

        public void InvokeMessage<T>(T message) where T : Message
        {
            if (CanRecieveMessage<T>())
            {
                messageResponses.TryGetValue(typeof(T).ToString(), out Action<Message> handler);
                handler.Invoke(message);
            }
        }

        public void HandleDamage(DamageMessage message)
        {
            //health -= message.damage;
            mover.Knockback(message.knockback, message.staggerDuration, message.staggerIntensity);
            message.consumed = true;
        }

        #endregion

    }
}
