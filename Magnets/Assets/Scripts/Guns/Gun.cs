using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float timeBetweenShots = 0.3f;
    [SerializeField] private Transform shootPos;
    [SerializeField] private BulletLine lineEffect;
    private Animator anims;
    bool canShoot = true;
    private void Awake()
    {
        anims = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerInput();
    }
    void PlayerInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (canShoot)
            {
                Fire();
                anims.SetTrigger("Shoot");
                Invoke(nameof(ResetShot), timeBetweenShots);
                canShoot = false;
            }
        }
    }
    void Fire()
    {
        Vector3 hitPoint = Vector3.zero;
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if(Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            hitPoint = hit.point;
            if(hit.collider.TryGetComponent(out IDamageable damageable))
            {
                float newDamage = damage;
                if (hit.collider.tag == "Head")
                    newDamage *= 1.5f;
                damageable.TakeDamage(newDamage);
            }
        }
        else
        {
            hitPoint = ray.GetPoint(100);
        }
        Vector3[] positions = new Vector3[2];
        positions[0] = shootPos.position;
        positions[1] = hitPoint;

        lineEffect.timer = 0;
        lineEffect.gameObject.SetActive(true);
        lineEffect.PlayEffect(positions);
    }
    void ResetShot()
    {
        canShoot = true;
    }
}