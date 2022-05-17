using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MagneticObject
{
    Grappling grapple;
    MagnetGuns gun;
    [SerializeField] private Transform grapplePoint;
    private void Awake()
    {
        grapple = FindObjectOfType<Grappling>();
    }
    public override void Connect(Transform conn, Transform mag, Pole pole)
    {
        base.Connect(conn, mag, pole);
        gun = connection.GetComponentInParent<MagnetGuns>();
        magnet.position = grapplePoint.position;
        magnet.rotation = grapplePoint.rotation;
        grapple.StartGrapple(grapplePoint.position, gun);
    }
    public override void Disconnect()
    {
        base.Disconnect();
        grapple.EndGrapple(gun);
        gun = null;
    }
}