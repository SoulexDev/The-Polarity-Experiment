using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvents : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent pickUpEvent;
    public void Interact()
    {
        pickUpEvent.Invoke();
    }
}