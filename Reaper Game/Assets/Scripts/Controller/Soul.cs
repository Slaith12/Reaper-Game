using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Movement;
using Reaper.Combat;
using Reaper.Messaging;

namespace Reaper.Enemy
{
    [RequireComponent(typeof(CombatTarget), typeof(Mover), typeof(WeaponUser))]
    public class Soul : MonoBehaviour, IMessageHandler
    {
        public EnemyInfo behavior;

        [HideInInspector] public Mover mover;
        [HideInInspector] public CombatTarget combatTarget;
        [HideInInspector] public WeaponUser weaponUser;
        [HideInInspector] public ComponentCache extraComponents;

        [HideInInspector] public Vector2 targetLocation;
        [HideInInspector] public float memoryTimer;
        [HideInInspector] public float morphTimer;
        [HideInInspector] public List<float> extraTimers;
        [HideInInspector] public int state;

        void Awake()
        {
            mover = GetComponent<Mover>();
            combatTarget = GetComponent<CombatTarget>();
            weaponUser = GetComponent<WeaponUser>();
            extraComponents = GetComponent<ComponentCache>();
            extraTimers = new List<float>();
        }

        protected virtual void Start()
        {
            behavior.InitState(this);
        }

        void Update()
        {
            behavior.UpdateSoul(this);
        }

        public bool CanRecieveMessage(string message)
        {
            throw new System.NotImplementedException();
        }

        public void InvokeMessage(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}
