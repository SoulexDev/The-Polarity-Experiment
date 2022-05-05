using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryEngine : MonoBehaviour, ISource
{
    [SerializeField] private Transform[] receivent;
    private List<IReceptical> receptical = new List<IReceptical>();

    [SerializeField] private MagneticObject.Pole pole;
    [SerializeField] private Transform batteryConn;
    [SerializeField] private Transform battery;
    [SerializeField] private AudioClip[] clips;

    private AudioSource[] sources;
    private void Awake()
    {
        foreach (Transform rec in receivent)
        {
            if (rec != null)
                receptical.Add(rec.GetComponent<IReceptical>());
        }
        sources = GetComponents<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(battery == null && other.tag == "Battery" && other.TryGetComponent(out MagneticObject magneticObject))
        {
            if (!(pole != magneticObject.pole))
                return;
            battery = other.transform;
            battery.GetComponent<MagneticObject>().Disconnect();
            battery.position = batteryConn.position;
            battery.rotation = batteryConn.rotation;
            battery.GetComponent<Rigidbody>().velocity = Vector3.zero;
            sources[1].PlayOneShot(clips[0]);
            sources[0].Play();
        }
    }
    private void Update()
    {
        if(battery != null && Vector3.Distance(battery.position, batteryConn.position) < 0.2f && receptical != null)
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
            if (battery != null)
                sources[1].PlayOneShot(clips[1]);
            sources[0].Stop();
            battery = null;
        }
    }
}