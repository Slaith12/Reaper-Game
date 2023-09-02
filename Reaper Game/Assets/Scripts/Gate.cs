using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Gate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().OnPlayerInteract += GameManager.LeaveHub;
    }
}
