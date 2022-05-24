using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [RequireComponent(typeof(DamageObject))]
    public class MeleeHit : MonoBehaviour
    {
        private float duration;

        public void Init(float duration, DamageObject.DamageHandler hitHandler)
        {
            this.duration = duration;
            GetComponent<DamageObject>().OnHit += hitHandler;
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }

        public static MeleeHit Create(float duration, DamageObject.DamageHandler hitHandler, Vector2 position, Vector2 size, List<string> targets, Transform parent = null, float rotation = 0)
        {
            Transform newObject = new GameObject("Melee").transform;
            newObject.parent = parent;
            newObject.position = position + (Vector2)newObject.parent.transform.position;
            newObject.eulerAngles = new Vector3(0, 0, rotation);
            newObject.localScale = size;
            newObject.gameObject.AddComponent<BoxCollider2D>();
            DamageObject damage = newObject.gameObject.AddComponent<DamageObject>();
            damage.targets = targets;
            MeleeHit melee = newObject.gameObject.AddComponent<MeleeHit>();
            melee.Init(duration, hitHandler);
            return melee;
        }
    }
}