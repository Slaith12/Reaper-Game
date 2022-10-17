using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Data Container/Entity")]
public class EntityData : ScriptableObject
{
    public new string name;
    public Sprite sprite;
}
