using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private PlayerController player;
    SpringJoint joint;
    MagnetGuns curGun;
    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void StartGrapple(Vector3 grapplePoint, MagnetGuns gun)
    {
        if (curGun == null)
            curGun = gun;
        if(gun != curGun)
        {
            MagnetGuns tGun = curGun;
            curGun = gun;
            if (tGun.conMag is GrapplePoint)
                return;
            tGun.RetractMagnet();
        }

        if(joint == null)
            joint = player.gameObject.AddComponent<SpringJoint>();

        player.grappling = true;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint;

        float ropeLength = Vector3.Distance(player.transform.position, grapplePoint);
        joint.maxDistance = ropeLength * 0.8f;
        joint.minDistance = ropeLength * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7;
        joint.massScale = 4.5f;
    }
    public void EndGrapple(MagnetGuns gun)
    {
        if (gun != curGun)
            return;
        player.grappling = false;
        Destroy(joint);
    }
}