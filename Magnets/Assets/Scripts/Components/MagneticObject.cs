using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public enum Pole { Positive, Negative, Both }
    public Pole pole;
    public Rigidbody rb;
    public Transform connection;
    public Transform magnet;
    public bool connected = false;

    public virtual void Connect(Transform conn, Transform mag)
    {
        connection = conn;
        magnet = mag;
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