using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField] private Animator anims;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 3)
            anims.SetTrigger("Slide");
    }
}