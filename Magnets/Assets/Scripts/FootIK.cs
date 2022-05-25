using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    [SerializeField] private AnimationCurve walkCurve;
    [SerializeField] private Transform foot;
    [SerializeField] private Transform body;
    [SerializeField] private FootIK otherFoot;
    [SerializeField] private float footOffset;
    [SerializeField] private float stepOffset = 3;
    Vector3 oldPos;
    Vector3 newPos;
    Vector3 hitPoint;
    float lerp = 1;
    float offset;
    bool footGrounded = true;
    private void Awake()
    {
        if(Physics.Raycast(foot.position, Vector3.down, out RaycastHit hit, 2, ~LayerMask.GetMask("Enemy")))
        {
            offset = hit.distance;
        }
        newPos = transform.position;
        oldPos = newPos;
    }
    private void Update()
    {
        transform.position = oldPos;

        Ray ray = new Ray(body.position + Vector3.up * 7.5f + (body.right * footOffset), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, ~LayerMask.GetMask("Enemy")))
        {
            hitPoint = hit.point + Vector3.up * offset;
            if (otherFoot.footGrounded && Vector3.Distance(newPos, hitPoint) > stepOffset)
            {
                lerp = 0;
                newPos = hitPoint;
            }
        }
        if (lerp < 1)
        {
            footGrounded = false;
            oldPos = Vector3.Lerp(oldPos, newPos, lerp);
            oldPos += Vector3.up * walkCurve.Evaluate(lerp);
            lerp += Time.deltaTime * 2;
        }
        else
        {
            footGrounded = true;
            lerp = 1;
            oldPos = newPos;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPos, 0.5f);
        Gizmos.DrawSphere(hitPoint, 0.25f);
    }
}