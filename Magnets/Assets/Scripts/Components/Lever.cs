using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MagneticObject, ISource
{
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();
    private AudioSource source;
    [SerializeField] private AudioClip clip;
    float curRot = 0;
    float prevRot = 0;
    bool activated { get { return curRot == 1 && receptical != null; } }
    private void Awake()
    {
        foreach (Transform rec in receivent)
        {
            if (rec != null)
                receptical.Add(rec.GetComponent<IReceptical>());
        }
        source = GetComponent<AudioSource>();
    }
    public override void Execute()
    {
        Vector3 connDir = connection.position - transform.position;
        Vector3 absoluteForward = Vector3.ProjectOnPlane(-transform.parent.forward, Vector3.up).normalized;
        curRot += 0.02f * (Vector3.Dot(absoluteForward, connDir) + 1) / 2;

        curRot = Mathf.Clamp01(curRot);
        transform.localRotation = Quaternion.Euler(new Vector3(curRot * -75, 0, 0));

        if (Mathf.Floor(curRot * 75) % 5 == 0 && curRot > 0 && curRot < 1)
        {
            if (Mathf.Floor(curRot * 75) == Mathf.Floor(prevRot * 75))
                return;
            source.PlayOneShot(clip);
        }
        prevRot = curRot;
    }
    private void Update()
    {
        if (activated)
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