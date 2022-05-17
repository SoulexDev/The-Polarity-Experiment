using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Activant")
        {
            other.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Activant")
        {
            other.transform.SetParent(null);
        }
    }
}