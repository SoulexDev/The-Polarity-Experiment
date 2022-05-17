using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandButton : MonoBehaviour, ISource
{
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();
    private int activantAmount = 0;

    [SerializeField] private MagneticObject.Pole pole;
    [SerializeField] private Material glowMat;
    private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    private Material[] mats;
    private Renderer rend;
    private float glowMatEmis = 1;
    bool active { get { return activantAmount > 0; } }
    private void Awake()
    {
        foreach (Transform rec in receivent)
        {
            if (rec != null)
                receptical.Add(rec.GetComponent<IReceptical>());
        }

        rend = transform.parent.GetComponent<Renderer>();
        mats = rend.sharedMaterials;
        mats[1] = glowMat = new Material(glowMat);
        rend.sharedMaterials = mats;
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Activant" && other.TryGetComponent(out MagneticObject magneticObject))
        {
            if (!(pole != magneticObject.pole))
                return;
            activantAmount++;
            if (activantAmount < 2)
                source.PlayOneShot(clips[0]);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Activant" && other.TryGetComponent(out MagneticObject magneticObject))
        {
            if (!(pole != magneticObject.pole))
                return;
            activantAmount--;
            if (activantAmount == 0)
                source.PlayOneShot(clips[1]);
        }
        if(other == null)
        {
            activantAmount--;
            if (activantAmount == 0)
                source.PlayOneShot(clips[1]);
        }
    }
    private void Update()
    {
        if (active)
        {
            foreach (var receptical in receptical)
            {
                receptical.Activate(this);
            }
            glowMatEmis = Mathf.Lerp(glowMatEmis, 5, Time.deltaTime * 5);
        }
        else
        {
            foreach (var receptical in receptical)
            {
                receptical.Deactivate(this);
            }
            glowMatEmis = Mathf.Lerp(glowMatEmis, 1, Time.deltaTime * 5);
        }
        glowMat.SetFloat("Emission_Power", glowMatEmis);
    }
}