using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] Color normalDisplayColor;
    [SerializeField] Color normalBackgroundColor;
    [SerializeField] Color overtimeDisplayColor;
    [SerializeField] Color overtimeBackgroundColor;
    [Space]
    [SerializeField] Image display;
    [SerializeField] Image background;

    private void Awake()
    {
        display.color = normalDisplayColor;
        background.color = normalBackgroundColor;
        display.fillAmount = 1;
    }

    private void Start()
    {
        ShiftManager.instance.OnOvertimeStart += delegate { display.color = overtimeDisplayColor; background.color = overtimeBackgroundColor; display.fillAmount = 1; };
    }

    private void Update()
    {
        display.fillAmount = ShiftManager.instance.GetTimePercentage();
    }
}
