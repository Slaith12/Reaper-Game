using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Shops
{
    [RequireComponent(typeof(Interactable))]
    public class Shop : MonoBehaviour
    {
        [SerializeField] ShopData shopData;

        private void Start()
        {
            Interactable interactable = GetComponent<Interactable>();
            interactable.OnPlayerInteract += delegate { ShopManager.instance.OpenShop(shopData); };
            interactable.OnPlayerLeave += delegate { ShopManager.instance.CloseShop(); };
        }
    }
}