using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPhysicsObject : MagneticObject
{
    private AudioSource source;
    Material mat;
    [SerializeField] private AudioClip collisionClip;
    [SerializeField] private int grabSpeed = 15;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        Renderer rend = GetComponent<Renderer>();
        mat = rend.sharedMaterial;
        mat = new Material(mat);
        rend.sharedMaterial = mat;
    }
    public override void Execute()
    {
        base.Execute();
        if(magnet != null)
            rb.AddForceAtPosition((connection.position - rb.position).normalized * Time.deltaTime * grabSpeed, magnet.position, ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 0.1f)
        {
            source.PlayOneShot(collisionClip);
        }
    }
    public void StartDisintegrate()
    {
        StartCoroutine(Disintegrate());
    }
    IEnumerator Disintegrate()
    {
        if(connection != null)
            connection.GetComponentInParent<MagnetGuns>().RetractMagnet();
        gameObject.layer = 2;
        float dAmount = mat.GetFloat("DissolveAmount");
        rb.useGravity = false;

        rb.velocity *= 0.1f;
        rb.angularVelocity *= 0.1f;

        while (dAmount < 1)
        {
            mat.SetFloat("DissolveAmount", dAmount += 0.01f);
            yield return null;
        }
        transform.position = new Vector3(999, 999, 999);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}