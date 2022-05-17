using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable, ISource
{
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();
    bool active = false;
    [SerializeField] private bool toggle = false;
    [SerializeField] private float timer = 0.1f;
    private AudioSource source;

    void Awake()
    {
        foreach (Transform rec in receivent)
        {
            receptical.Add(rec.GetComponent<IReceptical>());
        }
        source = GetComponent<AudioSource>();
    }
    public void Interact()
    {
        if (toggle)
            active = !active;
        else
            active = true;
        source.Play();
        if (active)
        {
            foreach (var receptical in receptical)
            {
                receptical.Activate(this);
            }

            if (!toggle)
            {
                Invoke(nameof(DeactivateReceptical), timer);
                active = false;
            }
        }
        else
        {
            DeactivateReceptical();
        }
    }
    void DeactivateReceptical()
    {
        foreach (var receptical in receptical)
        {
            receptical.Deactivate(this);
        }
    }
}