using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private const string HUBSCENE = "Hub Scene";
    private const string SHIFTSCENE = "SampleScene";

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public static void LeaveHub()
    {
        SceneManager.LoadScene(SHIFTSCENE);
    }

    public static void StartShift()
    {
        ShiftManager.Initialize();
        ShiftManager.BeginShift();
    }

    public static void EndShift()
    {
        EnterHub();
    }

    public static void EnterHub()
    {
        SceneManager.LoadScene(HUBSCENE);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
