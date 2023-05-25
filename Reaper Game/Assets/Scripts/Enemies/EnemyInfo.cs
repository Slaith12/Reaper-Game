using System;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Combat;
using Reaper.Data;
using Reaper.Items;

namespace Reaper.Enemy
{
    public abstract class EnemyInfo : ScriptableObject, MoverAttributes, SoftColliderAttributes
    {
        public new string name;
        public Sprite sprite;
        public ItemData captureItem;
        [SerializeField] GameObject prefab;
        [Space]
        public int maxHealth = 5;
        [SerializeField] float m_acceleration = 60;
        public virtual float acceleration => m_acceleration;
        [SerializeField] float m_friction = 30;
        public virtual float friction => m_friction;
        [Space]
        public float patrolSpeed = 5;
        public float chaseSpeed = 5;
        public float closeOptimalRange = 0.75f;
        public float farOptimalRange = 1.5f;
        [Space]
        [SerializeField] float m_knockbackMult = 1;
        public virtual float knockbackMult => m_knockbackMult;
        public virtual bool partialKnockback => false;
        [SerializeField] float m_pushForce = 25;
        public virtual float pushForce => m_pushForce;
        [Space]
        [SerializeField] float m_sightDistance = 7;
        public float sightDistance => m_sightDistance;
        [SerializeField] float m_chaseDistance = 10;
        public float chaseDistance => m_chaseDistance;
        public float memoryTime = 5;
        [Space]
        public Weapon mainWeapon;

        public virtual GameObject Create(Vector2 position)
        {
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.transform.position = position;
            newEnemy.GetComponent<SpriteRenderer>().sprite = sprite;
            newEnemy.GetComponent<AttributeContainer>().SetAttributes(this);
            return newEnemy;
        }
    }
}