using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraNoiseShake : MonoBehaviour
{
    [SerializeField] private float magnitude = 0.1f;
    [SerializeField] private float roughness = 0.1f;
    [SerializeField] private float fadeInTime = 0.1f;
    [SerializeField] private float fadeOutTime = 0.1f;
    void FixedUpdate()
    {
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
    }
}