using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreak : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rb;
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        foreach (Rigidbody rigidbody in rb)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(2000, transform.position, 10);
        }
        triggered = true;
    }
}