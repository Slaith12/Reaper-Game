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
        public ItemData itemType { get; private set; }

        public static Pickup Create(ItemData itemType, Vector2 position, params Type[] extraComponents)
        {
            return Create<Pickup>(itemType, position, extraComponents);
        }

        public static T Create<T>(ItemData itemType, Vector2 position, params Type[] extraComponents) where T : Pickup
        {
            GameObject newObject = new GameObject(itemType.name);
            newObject.transform.position = position;
            newObject.AddComponent<SpriteRenderer>().sprite = itemType.sprite;
            //collider added for pickup detection
            newObject.AddComponent<CircleCollider2D>().isTrigger = true;
            if (itemType.animator != null)
            {
                newObject.AddComponent<Animator>().runtimeAnimatorController = itemType.animator;
            }
            foreach(Type component in extraComponents)
            {
                newObject.AddComponent(component);
            }
            T newPickup = newObject.AddComponent<T>();
            newPickup.itemType = itemType;
            return newPickup;
        }
    }
}