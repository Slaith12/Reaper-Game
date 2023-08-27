using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Interactable currentInteractable;

    public void Interact()
    {
        if(currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    public void AddInteractable(Interactable newInteractable)
    {
        if(currentInteractable != null)
            RemoveInteractable(currentInteractable);
        currentInteractable = newInteractable;
        newInteractable.Select();
    }

    public void RemoveInteractable(Interactable oldInteractable)
    {
        if(currentInteractable == oldInteractable)
        {
            currentInteractable = null;
        }
        oldInteractable.Deselect();
    }
}
