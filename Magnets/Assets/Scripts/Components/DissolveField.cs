using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DissolveField : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 1;
    List<ISource> sources = new List<ISource>();
    bool isActivated { get { return sources.Count >= connectionAmount; } }

    [SerializeField] private Transform lEmitter;
    [SerializeField] private Transform rEmitter;
    [SerializeField] private Transform field;

    public bool invertPower = false;

    public void Activate(ISource source)
    {
        if (!sources.Contains(source))
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
    public void UpdateScale()
    {
        if(lEmitter != null && rEmitter != null)
            field.localScale = new Vector3(Vector3.Distance(lEmitter.position, rEmitter.position) / 4, 1, 1);
    }
    private void Update()
    {
        field.gameObject.SetActive(isActivated != invertPower);
    }
}