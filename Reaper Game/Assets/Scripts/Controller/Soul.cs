using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Movement;
using Reaper.Combat;

namespace Reaper.Controller
{
    [RequireComponent(typeof(CombatTarget))]
    [RequireComponent(typeof(Mover))]
    public class Soul : MonoBehaviour
    {
        [SerializeField] protected DamageObject contact;
        protected Mover mover;
        protected CombatTarget combatTarget;
        protected PlayerController player;

        [SerializeField] int maxHealth;
        [SerializeField] float maxSpeed = 5;
        [SerializeField] int damage = 1;
        [SerializeField] float knockbackStrength = 15;
        [SerializeField] float staggerDuration = 0.2f;
        [SerializeField] float sightDistance = 10f;
        [SerializeField] float memoryTime = 5f;
        [SerializeField] float patrolSpeed = 5f;
        protected float memoryTimer;
        private float patrolAngle;
        private float morphTimer;
        public bool morphed;

        void Awake()
        {
            mover = GetComponent<Mover>();
            combatTarget = GetComponent<CombatTarget>();
            combatTarget.OnDeath +=  Demorph;
            Demorph();
        }

        protected virtual void Start()
        {
            player = PlayerController.player;
            contact.OnHit += Damage;
        }

        void Update()
        {
            if (!morphed)
            {
                if (morphTimer > 0)
                {
                    morphTimer -= Time.deltaTime;
                }
                else
                {
                    Morph();
                }
            }
        }

        private void FixedUpdate()
        {
            if (morphed)
            {
                if ((player.transform.position - transform.position).magnitude <= sightDistance)
                    memoryTimer = memoryTime;
                else if (memoryTimer > 0)
                    memoryTimer -= Time.deltaTime;
                if (memoryTimer > 0)
                    Chase();
                else
                    Patrol();
            }
        }

        protected virtual void Chase()
        {
            mover.targetSpeed = (player.transform.position - transform.position).normalized * maxSpeed;
        }

        protected virtual void Patrol()
        {
            patrolAngle += Random.Range(-20 * Time.deltaTime, 20 * Time.deltaTime);
            mover.targetSpeed = patrolAngle.ToDirection() * patrolSpeed;
        }

        private void Morph()
        {
            morphed = true;
            combatTarget.health = maxHealth;
            combatTarget.invuln = false;
        }

        private void Demorph()
        {
            morphTimer = 5;
            morphed = false;
            combatTarget.invuln = true;
            mover.targetSpeed = new Vector2(0, 0);
        }

        private void Damage(Collider2D collision)
        {
            if (!morphed)
                return;
            Debug.Log("Player Hit");
            //Demorph();
            collision.GetComponent<CombatTarget>()?.Damage(damage, (player.transform.position - transform.position).normalized * knockbackStrength, staggerDuration);
        }
    }
}
