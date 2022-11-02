using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentCache : MonoBehaviour
{
    public Component[] components;

    public Component this[int i] => components[i];
}
