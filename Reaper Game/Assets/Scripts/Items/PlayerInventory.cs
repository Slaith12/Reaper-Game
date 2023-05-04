using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Items
{
    public class PlayerInventory : MonoBehaviour
    {
        public Inventory inventory { get; private set; }

        private void Awake()
        {
            inventory = new Inventory();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Pickup pickup = collision.GetComponent<Pickup>();
            if (pickup == null)
            {
                return;
            }
            PickupItem(pickup);
        }

        public void PickupItem(Pickup pickup)
        {
            inventory.AddItem(pickup.itemType);
            Destroy(pickup.gameObject);
        }
    }
}