using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 0;
    [SerializeField] private Laser laser;
    List<ISource> sources = new List<ISource>();
    bool isActivated { get { return sources.Count >= connectionAmount; } }
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

    public void OverrideDeactivate()
    {
        connectionAmount = 50;
    }
    private void Update()
    {
        if (isActivated)
        {
            if (!laser.gameObject.activeSelf)
                laser.gameObject.SetActive(true);
        }
        else
        {
            if (laser.gameObject.activeSelf)
            {
                if (laser.receptical != null)
                    laser.receptical.Deactivate(laser);
                laser.gameObject.SetActive(false);
            }
        }
    }
}