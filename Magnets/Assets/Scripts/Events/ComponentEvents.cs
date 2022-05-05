using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComponentEvents : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 1;
    [SerializeField] private UnityEvent compEvent;
    List<ISource> sources = new List<ISource>();
    bool eventPlayed = false;
    bool active { get { return sources.Count >= connectionAmount; } }

    public void Activate(ISource source)
    {
        if (sources.Contains(source))
            return;
        else
            sources.Add(source);
    }

    public void Deactivate(ISource source)
    {
        if (sources.Contains(source))
            sources.Remove(source);
    }

    public void OverrideActivate()
    {
        connectionAmount = 0;
    }

    private void Update()
    {
        if(active && !eventPlayed)
        {
            compEvent.Invoke();
            eventPlayed = true;
        }
    }
}