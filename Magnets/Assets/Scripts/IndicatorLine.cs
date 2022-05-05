using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IndicatorLine : MonoBehaviour
{
    [SerializeField] private Transform from;
    [SerializeField] private Transform to;
    [SerializeField] private LineRenderer line;

    List<Vector3> points = new List<Vector3>();

    public bool reset = false;

    private void Update()
    {
        points.Clear();
        points.Add(from.position);
        points.Add(new Vector3(from.position.x, from.position.y, to.position.z));
        if(to.position.y > from.position.y)
            points.Add(new Vector3(from.position.x, to.position.y, to.position.z));
        points.Add(to.position);

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}