using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 1;
    [SerializeField] private Transform dropPos;
    [SerializeField] private GameObject currentObject;
    public enum DropType { PositiveCube, NegativeCube, PositiveBattery, NegativeBattery, PositiveRefractionCube, NegativeRefractionCube }
    [SerializeField] private DropType dropType;
    [SerializeField] private PrefabTypePair[] typePairs;

    List<ISource> sources = new List<ISource>();
    bool isActivated { get { return sources.Count >= connectionAmount; } }
    bool dropped = false;

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

    private void Update()
    {
        if(currentObject == null)
        {
            currentObject = Instantiate(SearchForItem(dropType), dropPos);
        }
        if (isActivated && !dropped)
        {
            currentObject.GetComponent<MagnetPhysicsObject>().StartDisintegrate();
            dropped = true;
        }
        else
        {
            dropped = false;
        }
    }

    public GameObject SearchForItem(DropType type)
    {
        for (int i = 0; i < typePairs.Length; i++)
        {
            if(typePairs[i].dropType == type)
            {
                return typePairs[i].dropItem;
            }
        }
        return null;
    }
}

[System.Serializable]
public class PrefabTypePair
{
    public Dropper.DropType dropType;
    public GameObject dropItem;
}