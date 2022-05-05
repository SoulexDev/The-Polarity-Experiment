using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour
{
    [SerializeField] private Transform cam;

    [SerializeField] private Transform gunOne;
    [SerializeField] private Transform gunTwo;
    [SerializeField] private Transform gunFollowOne;
    [SerializeField] private Transform gunFollowTwo;
    void Start()
    {
        
    }
    void LateUpdate()
    {
        transform.position = cam.position;
        transform.rotation = cam.rotation;
        gunOne.position = gunFollowOne.position;
        gunTwo.position = gunFollowTwo.position;
    }
}