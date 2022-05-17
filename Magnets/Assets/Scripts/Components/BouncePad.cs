using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private Vector3 bounceForce = Vector3.up * 10;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController controller))
        {
            controller.AddForce(bounceForce);
        }
        if(other.TryGetComponent(out Rigidbody rb) && !rb.isKinematic)
        {
            rb.AddForce(bounceForce, ForceMode.Impulse);
        }
    }
}