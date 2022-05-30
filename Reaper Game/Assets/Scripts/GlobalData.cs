using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance { get; private set; }

    public string[] enemyNames;
    public Sprite[] enemySprites;

    public string[] itemNames;
    public Sprite[] itemSprites;

    private void Awake()
    {
        instance = this;
    }
}
