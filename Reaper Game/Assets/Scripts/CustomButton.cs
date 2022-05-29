using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public delegate void SelectEvent();
    public event SelectEvent OnSelection;
    public event SelectEvent OnDeselection;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnSelection?.Invoke();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        OnDeselection?.Invoke();
    }
}
