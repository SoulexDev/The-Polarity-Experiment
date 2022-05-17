using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvents : MonoBehaviour, IInteractable
{
    public UnityEvent interactEvent;
    bool eventPlayed = false;

    public void Interact()
    {
        if (eventPlayed)
            return;
        interactEvent.Invoke();
        eventPlayed = true;
    }
}