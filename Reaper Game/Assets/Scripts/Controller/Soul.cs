using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Movement;
using Reaper.Combat;

namespace Reaper.Enemy
{
    [RequireComponent(typeof(CombatTarget))]
    [RequireComponent(typeof(Mover))]
    public class Soul : MonoBehaviour
    {
        public EnemyInfo behavior;

        [HideInInspector] public Mover mover;
        [HideInInspector] public CombatTarget combatTarget;
        [HideInInspector] public ComponentCache extraComponents;

        [HideInInspector] public float memoryTimer;
        [HideInInspector] public float morphTimer;
        [HideInInspector] public List<float> extraTimers;
        [HideInInspector] public int state;

        void Awake()
        {
            mover = GetComponent<Mover>();
            combatTarget = GetComponent<CombatTarget>();
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
    }
}
