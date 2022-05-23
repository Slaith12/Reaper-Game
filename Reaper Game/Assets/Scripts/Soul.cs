using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Movement;
using Reaper.Combat;

namespace Reaper.Controller
{
    [RequireComponent(typeof(Mover))]
    public class Soul : MonoBehaviour
    {
        [SerializeField] DamageObject contact;
        private Mover mover;
        private PlayerController player;

        [SerializeField] float maxSpeed = 5;
        [SerializeField] float knockbackStrength = 15;
        private float morphTimer;
        private bool morphed;

        void Awake()
        {
            morphTimer = 5;
            morphed = false;
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            player = PlayerController.player;
            contact.OnHit += Damage;
        }

        void Update()
        {
            if (morphTimer > 0)
            {
                morphTimer -= Time.deltaTime;
            }
            else
            {
                morphed = true;
            }
        }

        private void FixedUpdate()
        {
            if (morphed)
            {
                mover.targetSpeed = (player.transform.position - transform.position).normalized * maxSpeed;
            }
        }

        private void Demorph()
        {
            morphTimer = 5;
            morphed = false;
            mover.targetSpeed = new Vector2(0, 0);
        }

        private void Damage(Collider2D collision)
        {
            if (!morphed)
                return;
            Debug.Log("Player Hit");
            //Demorph();
            collision.GetComponent<Mover>().Knockback((player.transform.position - transform.position).normalized * knockbackStrength, 0.2f);
        }
    }
}
