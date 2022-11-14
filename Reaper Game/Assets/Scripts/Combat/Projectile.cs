using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public class Projectile : DamageObject
    {
        protected new Rigidbody2D rigidbody;
        protected new SpriteRenderer renderer;
        protected float duration;

        public event DamageHandler OnEnvironmentHit;
        protected List<string> environmentTargets;
        public static readonly List<string> defaultEnviroTargets = new List<string> { "Environment" };

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody == null)
                rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            renderer = GetComponent<SpriteRenderer>();
            if (renderer == null)
                renderer = gameObject.AddComponent<SpriteRenderer>();
        }

        public virtual void Init(Sprite sprite, List<string> hitTargets, List<string> environmentTargets, Vector2 velocity, float duration)
        {
            renderer.sprite = sprite;
            targets = hitTargets;
            this.environmentTargets = environmentTargets;
            rigidbody.velocity = velocity;
            this.duration = duration;
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
                Destroy(gameObject);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (environmentTargets.Contains(collision.tag))
            {
                OnEnvironmentHit?.Invoke(collision, this);
            }
        }

        public static Projectile Create(Sprite sprite, List<string> hitTargets, List<string> environmentTargets, Vector2 velocity, float duration)
        {
            Projectile projectile = new GameObject("Projectile", typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Projectile)).GetComponent<Projectile>();
            projectile.Init(sprite, hitTargets, environmentTargets, velocity, duration);
            return projectile;
        }
    }
}
