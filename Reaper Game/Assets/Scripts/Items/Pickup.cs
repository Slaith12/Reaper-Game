using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Items
{
    public class Pickup : MonoBehaviour
    {
        /// <summary>
        /// The item that this pickup corresponds to.
        /// </summary>
        public ItemData item;

        public static Pickup Create(ItemData item, Vector2 position, params Type[] extraComponents)
        {
            return Create<Pickup>(item, position, extraComponents);
        }

        public static T Create<T>(ItemData item, Vector2 position, params Type[] extraComponents) where T : Pickup
        {
            GameObject newObject = new GameObject(item.name);
            newObject.transform.position = position;
            newObject.AddComponent<SpriteRenderer>().sprite = item.sprite;
            if (item.animator != null)
            {
                newObject.AddComponent<Animator>().runtimeAnimatorController = item.animator;
            }
            foreach(Type component in extraComponents)
            {
                newObject.AddComponent(component);
            }
            T newPickup = newObject.AddComponent<T>();
            return newPickup;
        }
    }
}