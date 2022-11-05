using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [RequireComponent(typeof(DamageObject))]
    public class MeleeHit : MonoBehaviour
    {
        private float duration;
        private List<Collider2D> prevHits;

        public void Init(float duration, DamageHandler hitHandler)
        {
            this.duration = duration;
            prevHits = new List<Collider2D>();
            GetComponent<DamageObject>().OnHit += c => validateHit(c, hitHandler);
        }

        private void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void validateHit(Collider2D collision, DamageHandler hitHandler)
        {
            if (prevHits.Contains(collision))
                return;
            prevHits.Add(collision);
            hitHandler?.Invoke(collision);
        }

        public static MeleeHit Create(float duration, DamageHandler hitHandler, Vector2 offset, Vector2 size, List<string> targets, Transform parent = null, float rotation = 0)
        {
            Transform newObject = new GameObject("Melee").transform;
            newObject.parent = parent;
            newObject.position = offset;
            if(newObject.parent != null)
            {
                newObject.position += newObject.parent.position;
            }
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