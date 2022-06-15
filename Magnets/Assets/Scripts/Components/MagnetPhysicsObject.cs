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
        {
            Vector3 dir = (connection.position - rb.position).normalized;
            Vector3 multDir;
            if (Physics.Linecast(rb.position, connection.position, out RaycastHit hit))
            {
                multDir = Vector3.ProjectOnPlane(dir, hit.normal).normalized;
                dir = multDir;
            }
            rb.AddForceAtPosition(dir * Time.deltaTime * grabSpeed, magnet.position, ForceMode.VelocityChange);
        }
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