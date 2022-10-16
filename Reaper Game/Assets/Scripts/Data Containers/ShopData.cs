using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop", menuName = "Data Container/Shop")]
public class ShopData : ScriptableObject
{
    private List<ShopItem> items;
    private List<Contract> contracts;

    private void Awake()
    {
        if(items != null || contracts != null)
        {
            Debug.Log("Post-initialization Awake");
            return;
        }
        items = new List<ShopItem>();
        contracts = new List<Contract>();
    }
}
