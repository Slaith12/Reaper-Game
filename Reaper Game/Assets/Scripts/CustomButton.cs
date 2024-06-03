using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public event Action OnSelection;
    public event Action OnDeselection;

    bool selected;
    protected override void Awake()
    {
        base.Awake();
        selected = false;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        if (!selected)
        {
            OnSelection?.Invoke();
            selected = true;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!selected)
        {
            OnSelection?.Invoke();
            selected = true;
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        if (selected)
        {
            OnDeselection?.Invoke();
            selected = false;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (selected)
        {
            OnDeselection?.Invoke();
            selected = false;
        }
    }
}
