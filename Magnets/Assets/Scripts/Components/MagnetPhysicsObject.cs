using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPhysicsObject : MagneticObject
{
    private AudioSource source;
    [SerializeField] private AudioClip collisionClip;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public override void Execute()
    {
        base.Execute();
        if(magnet != null)
            rb.AddForceAtPosition((connection.position - rb.position).normalized * 10, magnet.position, ForceMode.Force);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 0.1f)
        {
            source.PlayOneShot(collisionClip);
        }
    }
}