using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop", menuName = "Data Container/Shop")]
public class ShopData : ScriptableObject
{
    public List<ShopItem> items;
    public List<Contract> contracts;

    private void Awake()
    {
        if(items != null || contracts != null)
        {
            return;
        }
        items = new List<ShopItem>();
        contracts = new List<Contract>();
    }
}
