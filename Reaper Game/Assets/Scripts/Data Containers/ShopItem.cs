using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Data Container/Shop Item")]
public class ShopItem : ScriptableObject
{
    public ItemData item;
    public string flavorText;
    public int price;
}
