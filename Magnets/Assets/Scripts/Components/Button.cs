using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable, ISource
{
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();
    bool active = false;
    private AudioSource source;

    void Awake()
    {
        foreach (Transform rec in receivent)
        {
            if (rec != null)
                receptical.Add(rec.GetComponent<IReceptical>());
        }
        source = GetComponent<AudioSource>();
    }
    public void Interact()
    {
        active = !active;
        source.Play();
    }
    void Update()
    {
        if (active)
        {
            foreach (var receptical in receptical)
            {
                receptical.Activate(this);
            }
        }
        else
        {
            foreach (var receptical in receptical)
            {
                receptical.Deactivate(this);
            }
        }
    }
}