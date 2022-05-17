using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEngine : MonoBehaviour, IReceptical, ISource
{
    public MagneticObject.Pole pole;
    [SerializeField] private int connectionAmount = 0;
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();
    List<ISource> sources = new List<ISource>();
    bool isActivated { get { return sources.Count >= connectionAmount; } }
    private void Awake()
    {
        foreach (Transform rec in receivent)
        {
            if (rec != null)
                receptical.Add(rec.GetComponent<IReceptical>());
        }
    }
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
            if (receptical != null)
            {
                foreach (var receptical in receptical)
                {
                    receptical.Activate(this);
                }
            }
        }
        else
        {
            if (receptical != null)
            {
                foreach (var receptical in receptical)
                {
                    receptical.Deactivate(this);
                }
            }
        }
    }
}