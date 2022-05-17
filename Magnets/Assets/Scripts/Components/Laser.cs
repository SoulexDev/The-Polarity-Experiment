using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, ISource
{
    [SerializeField] private MagneticObject.Pole pole;
    private LineRenderer laserRenderer;
    public IReceptical receptical;
    Vector3 hitPoint;
    private void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 100, ~LayerMask.GetMask("Player", "Glass", "Ignore Raycast")))
        {
            hitPoint = hit.point;
            Transform hitTransform = hit.transform;

            if (hitTransform.TryGetComponent(out IReceptical temp))
            {
                if (receptical != null && receptical != temp)
                    receptical.Deactivate(this);
                if (temp != null)
                {
                    if (hitTransform.TryGetComponent(out MagneticObject magneticObject) && magneticObject.pole == pole || hitTransform.TryGetComponent(out LaserEngine laserEngine) && laserEngine.pole == pole)
                    {
                        receptical = temp;
                    }
                    else
                        receptical = null;
                }
                if(receptical != null)
                    receptical.Activate(this);
            }
            else if(receptical != null)
            {
                receptical.Deactivate(this);
            }
        }
    }
    private void LateUpdate()
    {
        laserRenderer.SetPosition(0, transform.position);
        laserRenderer.SetPosition(1, hitPoint);
    }
}