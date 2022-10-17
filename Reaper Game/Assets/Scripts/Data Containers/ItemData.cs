using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Data Container/Item")]
public class ItemData : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public string description;
}
