using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MagneticObject, ISource
{
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();
    private AudioSource[] source;
    [SerializeField] private AudioClip[] clips;

    [SerializeField] private Transform connectionPoint;
    [SerializeField] private Material glowMat;
    [SerializeField] private bool kickStart = false;
    private Material[] mats;
    private Renderer rend;
    private float glowMatEmis = 1;
    bool active = false;
    private void Awake()
    {
        foreach (Transform rec in receivent)
        {
            if (rec != null)
                receptical.Add(rec.GetComponent<IReceptical>());
        }

        rend = GetComponent<Renderer>();
        mats = rend.sharedMaterials;
        mats[1] = glowMat = new Material(glowMat);
        rend.sharedMaterials = mats;
        source = GetComponents<AudioSource>();
    }
    public override void Connect(Transform conn, Transform mag, Pole pole)
    {
        base.Connect(conn, mag, pole);
        magnet.position = connectionPoint.position;
        magnet.rotation = connectionPoint.rotation * Quaternion.Euler(new Vector3(90, 0, 0));
    }
    public override void Execute()
    {
        if (!active)
        {
            source[1].PlayOneShot(clips[0]);
            source[0].Play();
        }
        active = true;
    }
    public override void StopExecute()
    {
        if (!kickStart)
        {
            if (active)
            {
                source[1].PlayOneShot(clips[1]);
                source[0].Stop();
            }
            active = false;
        }
    }
    private void Update()
    {
        if (active)
        {
            if (receptical != null)
            {
                foreach (var receptical in receptical)
                {
                    receptical.Activate(this);
                }
            }
            glowMatEmis = Mathf.Lerp(glowMatEmis, 5, Time.deltaTime * 5);
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
            glowMatEmis = Mathf.Lerp(glowMatEmis, 1, Time.deltaTime * 5);
        }

        glowMat.SetFloat("Emission_Power", glowMatEmis);
    }
}