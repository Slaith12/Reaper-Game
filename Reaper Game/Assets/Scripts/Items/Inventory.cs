using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Items
{
    public class Inventory
    {
        //readonly means the list can't be replaced. it doesn't mean we can't change the list's contents
        private readonly IDictionary<ItemData, int> items;

        public int this[ItemData item] => GetItemQuantity(item);
        public int numUniqueItems => items.Keys.Count;
        public int totalItems => GetTotalItems();

        public Inventory()
        {
            items = new Dictionary<ItemData, int>();
        }

        public void AddItem(ItemData item, int quantity = 1)
        {
            if (quantity <= 0) 
            {
                Debug.Log("Please only input positive numbers to AddItem(). If you want to subtract items, use TrySubtractItem.\n" +
                    "If you want to subtract items while allowing the item quantity to go negative, add \"allowNegative: true\" to the function call.");
            }
            if(!items.ContainsKey(item))
            {
                items.Add(item, 0);
            }

            items[item] += quantity;
            if (items[item] == 0)
            {
                items.Remove(item);
            }
        }

        public bool TrySubtractItem(ItemData item, int quantity = 1, bool allowNegative = false)
        {
            if (!allowNegative && (!items.ContainsKey(item) || items[item] < quantity)) 
            {
                return false;
            }
            if(!items.ContainsKey(item))
            {
                items.Add(item, 0);
            }

            items[item] -= quantity;
            if (items[item] == 0) 
            {
                items.Remove(item);
            }
            return true;
        }

        public bool ContainsItem(ItemData item, bool positiveOnly = true)
        {
            return items.ContainsKey(item) && (!positiveOnly || items[item] > 0);
        }

        public int GetItemQuantity(ItemData item)
        {
            if (!items.ContainsKey(item))
                return 0;
            return items[item];
        }

        public int GetTotalItems()
        {
            int total = 0;
            foreach (int count in items.Values)
            {
                total += count;
            }
            return total;
        }
    }
}
