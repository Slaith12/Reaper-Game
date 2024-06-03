using Reaper.Environment;
using System;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    public static ShiftManager instance { get; private set; }

    [SerializeField] float baseShiftTime;
    [SerializeField] float overtimeShiftTime;
    [SerializeField] private float m_shiftTimer;
    private bool m_started;
    private bool m_overtime;

    public float shiftTimer => m_shiftTimer;
    public bool started => m_started;
    public bool overtime => m_overtime;

    public event Action OnOvertimeStart;

    private EnemySpawner spawner;

    private void Awake()
    {
        instance = this;
        spawner = GetComponent<EnemySpawner>();
        spawner.enabled = false;
    }

    private void Start() //debug only
    {
        GameManager.StartShift();
    }

    public void Initialize() //to be called by loading screen
    {
        m_shiftTimer = baseShiftTime;
    }

    public void BeginShift() //to be called by GameManager
    {
        m_started = true;
        spawner.enabled = true;
    }

    void Update()
    {
        if (!m_started)
            return;
        m_shiftTimer -= Time.deltaTime;
        if (m_shiftTimer < 0)
        {
            if(!m_overtime)
            {
                m_overtime = true;
                m_shiftTimer = overtimeShiftTime;
                OnOvertimeStart?.Invoke();
            }
            else
            {
                GameManager.EndShift();
            }
        }
    }

    public float GetTimePercentage()
    {
        return m_shiftTimer / (m_overtime ? overtimeShiftTime : baseShiftTime);
    }
}
