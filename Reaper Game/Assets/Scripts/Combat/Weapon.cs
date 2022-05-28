using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public abstract class Weapon : MonoBehaviour
    {
        private new Renderer renderer;

        protected virtual void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        public abstract void Attack(Vector2 facing);

        public void Enable()
        {
            if (renderer != null)
                renderer.enabled = true;
        }

        public void Disable()
        {
            if (renderer != null)
                renderer.enabled = false;
        }
    }
}
