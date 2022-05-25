using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private Rigidbody rb;
    public void Shoot(Vector3 direction)
    {
        rb.AddForce(direction * speed * 100);
    }
}