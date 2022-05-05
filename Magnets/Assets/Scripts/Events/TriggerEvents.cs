using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent trigEvent;
    bool eventPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!eventPlayed && other.gameObject.layer == 3)
        {
            trigEvent.Invoke();
            eventPlayed = true;
        }
    }
}