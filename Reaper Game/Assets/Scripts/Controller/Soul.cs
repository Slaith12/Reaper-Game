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
        [SerializeField] DamageObject contact;
        private Mover mover;
        private CombatTarget combatTarget;
        private PlayerController player;

        [SerializeField] int maxHealth;
        [SerializeField] float maxSpeed = 5;
        [SerializeField] int damage = 1;
        [SerializeField] float knockbackStrength = 15;
        [SerializeField] float staggerDuration = 0.2f;
        private float morphTimer;
        public bool morphed;

        void Awake()
        {
            mover = GetComponent<Mover>();
            combatTarget = GetComponent<CombatTarget>();
            combatTarget.OnDeath +=  Demorph;
            Demorph();
        }

        private void Start()
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
                mover.targetSpeed = (player.transform.position - transform.position).normalized * maxSpeed;
            }
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
