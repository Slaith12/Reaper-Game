using Reaper.Environment;
using UnityEngine;

public class ShiftManager : MonoBehaviour
{
    private static ShiftManager instance;

    [SerializeField] float baseShiftTime;
    [SerializeField] private float shiftTimer;
    private bool started;

    EnemySpawner spawner;

    private void Awake()
    {
        instance = this;
        spawner = GetComponent<EnemySpawner>();
        spawner.enabled = false;
    }

    private void Start()
    {
        GameManager.StartShift();
    }

    public static void Initialize()
    {
        instance.Init();
    }

    private void Init()
    {
        shiftTimer = baseShiftTime;
    }

    public static void BeginShift()
    {
        instance.Begin();
    }

    private void Begin()
    {
        started = true;
        spawner.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
            return;
        shiftTimer -= Time.deltaTime;
        if (shiftTimer < 0)
            GameManager.EndShift();
    }
}
