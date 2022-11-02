using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Shops
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] ShopData shopData;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player")
                return;
            ShopManager.instance.atShop = shopData;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag != "Player")
                return;
            if (ShopManager.instance.atShop == shopData)
                ShopManager.instance.atShop = null;
        }
    }
}