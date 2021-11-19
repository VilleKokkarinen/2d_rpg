using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public bool IsInteracting { get; set; }

    [SerializeField]
    private Window window;

    public virtual void Interact()
    {
        if(!IsInteracting)
        {
            IsInteracting = true;
            window.Open(this);
        }
    }

    public virtual void StopInteract()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            window.Close();
        }
    }
}
