using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public event Action OnPlayerApproach;
    public event Action OnPlayerLeave;

    public event Action OnPlayerSelect;
    public event Action OnPlayerDeselect;

    public event Action OnPlayerInteract;

    public void Select()
    {
        OnPlayerSelect?.Invoke();
    }

    public void Deselect()
    {
        OnPlayerDeselect?.Invoke();
    }

    public void Interact()
    {
        OnPlayerInteract?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInteract player = collision.GetComponent<PlayerInteract>();
        if (player == null)
            return;
        player.AddInteractable(this);
        OnPlayerApproach?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInteract player = collision.GetComponent<PlayerInteract>();
        if (player == null)
            return;
        player.RemoveInteractable(this);
        OnPlayerLeave?.Invoke();
    }
}
