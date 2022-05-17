using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MagnetPhysicsObject magnetPhysicsObject))
        {
            magnetPhysicsObject.StartDisintegrate();
        }
    }
}