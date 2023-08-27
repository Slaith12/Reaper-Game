using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Interactable))]
public class Gate : MonoBehaviour
{
    [SerializeField] string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().OnPlayerInteract += ChangeScene;
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
