using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get { if (m_instance == null) new GameObject("Game Manager", typeof(GameManager)); return m_instance; }
    }

    private const string HUBSCENE = "Hub Scene";
    private const string SHIFTSCENE = "SampleScene";

    private void Awake()
    {
        if(m_instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        DontDestroyOnLoad(gameObject);
        m_instance = this;
    }

    /// <summary>
    /// Called by loading zone when player leaves to go to the shift
    /// </summary>
    public static void LeaveHub()
    {
        SceneManager.LoadScene(SHIFTSCENE);
    }

    /// <summary>
    /// Called by load screen when player leaves truck and enters shift
    /// </summary>
    public static void StartShift()
    {
        ShiftManager.instance.Initialize();
        ShiftManager.instance.BeginShift();
    }

    /// <summary>
    /// Called by loading zone when player leaves to go back to hub
    /// </summary>
    public static void EndShift()
    {
        EnterHub();
    }

    /// <summary>
    /// Called by load screen when player leaves truck and enters hub
    /// </summary>
    public static void EnterHub()
    {
        SceneManager.LoadScene(HUBSCENE);
    }

    private void OnDestroy()
    {
        m_instance = null;
    }
}
