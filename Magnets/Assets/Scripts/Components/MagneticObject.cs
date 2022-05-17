using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public enum Pole { Positive, Negative, Both }
    public Pole pole;
    [HideInInspector] public Pole conPole;
    public Rigidbody rb;
    public Transform connection;
    public Transform magnet;
    public bool connected = false;

    public virtual void Connect(Transform conn, Transform mag, Pole pole)
    {
        connection = conn;
        magnet = mag;
        conPole = pole;
        magnet.SetParent(transform);
        connected = true;
    }
    public virtual void Execute()
    {

    }
    public virtual void StopExecute()
    {

    }
    public virtual void Disconnect()
    {
        if(magnet != null)
            magnet.SetParent(null);
        magnet = null;
        connection = null;
        connected = false;
    }
}