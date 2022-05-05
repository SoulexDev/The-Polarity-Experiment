using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent timeEvent;

    public void StartTimer(float time)
    {
        Invoke("PlayEvent", time);
    }
    void PlayEvent()
    {
        timeEvent.Invoke();
    }
}