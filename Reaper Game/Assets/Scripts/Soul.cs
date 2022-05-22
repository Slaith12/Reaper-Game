using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Reaper.Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Soul : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5;
        private float morphTimer;
        private bool morphed;

        private new Rigidbody2D rigidbody;
        private PlayerController player;

        void Awake()
        {
            morphTimer = 5;
            morphed = false;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            player = PlayerController.player;
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
                rigidbody.velocity = (player.transform.position - transform.position).normalized * maxSpeed;
            }
        }

        private void Demorph()
        {
            morphTimer = 5;
            morphed = false;
            rigidbody.velocity = new Vector2(0, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player")
                return;
            if (!morphed)
                return;
            Debug.Log("Player Hit");
            Demorph();
        }
    }
}
