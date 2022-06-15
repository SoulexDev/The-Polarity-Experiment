using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MagneticObject
{
    Grappling grapple;
    MagnetGuns magGun;
    [SerializeField] private Transform grapplePoint;
    private void Awake()
    {
        grapple = FindObjectOfType<Grappling>();
    }
    public override void Connect(MagnetGuns gun, Transform conn, Transform mag, Pole pole)
    {
        base.Connect(gun, conn, mag, pole);
        magGun = connection.GetComponentInParent<MagnetGuns>();
        magnet.position = grapplePoint.position;
        magnet.rotation = grapplePoint.rotation;
        grapple.StartGrapple(grapplePoint.position, gun);
    }
    public override void Disconnect()
    {
        base.Disconnect();
        grapple.EndGrapple(magGun);
        magGun = null;
    }
}