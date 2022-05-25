using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPad : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 0;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    [SerializeField] bool stopAtCheckPoints;
    [SerializeField] bool invertPower = false;
    //bool activeStopped;
    //bool deactivatedStopped;
    bool startCondition = false;

    bool checkedDir;
    enum Direction { forward = 1, backward = -1 }
    Direction direction;
    int curWayPoint = 0;
    private AudioSource source;
    List<ISource> sources = new List<ISource>();
    bool isActivated { get { return sources.Count >= connectionAmount; } }
    //bool audioPlayed = false;
    private void Awake()
    {
        waypoints.Insert(0, transform.position);
        source = GetComponent<AudioSource>();
        direction = Direction.forward;
    }
    public void Activate(ISource source)
    {
        if (!sources.Contains(source))
            sources.Add(source);
    }

    public void Deactivate(ISource source)
    {
        if (sources.Contains(source))
            sources.Remove(source);
    }
    public void OverrideActivate()
    {
        connectionAmount = 0;
    }
    public void OverrideDeactivate()
    {
        connectionAmount = 50;
    }
    private void Update()
    {
        //if (isActivated)
        //{
        //    deactivatedStopped = false;
        //    if (activeStopped)
        //        return;
        //    if (transform.position != waypoints[curWayPoint])
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, waypoints[curWayPoint], speed * Time.deltaTime * 10);
        //    }
        //    else
        //    {
        //        //ReverseEnum();
        //        CheckDir();
        //        if (stopAtCheckPoints)
        //            activeStopped = true;
        //    }
        //}
        //else
        //{
        //    activeStopped = false;
        //    if (!stopAtCheckPoints || deactivatedStopped)
        //        return;
        //    if (transform.position != waypoints[curWayPoint])
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, waypoints[curWayPoint], speed * Time.deltaTime * 10);
        //    }
        //    else
        //    {
        //        //ReverseEnum();
        //        CheckDir();
        //        deactivatedStopped = true;
        //    }
        //}
        bool active = isActivated != invertPower;
        if (stopAtCheckPoints)
        {
            if (active)
            {
                if (!startCondition)
                {
                    curWayPoint = 1;
                    startCondition = true;
                }
                if (checkedDir)
                {
                    CheckDir();
                    checkedDir = false;
                }
                if (transform.position != waypoints[curWayPoint])
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[curWayPoint], speed * Time.deltaTime * 10);
                else if(curWayPoint != waypoints.Count - 1)
                {
                    CheckDir();
                }
            }
            else
            {
                if (!checkedDir && startCondition)
                {
                    CheckDir();
                    checkedDir = true;
                }
                if (transform.position != waypoints[curWayPoint])
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[curWayPoint], speed * Time.deltaTime * 10);
            }
        }
        else
        {
            if (active)
            {
                if (transform.position != waypoints[curWayPoint])
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[curWayPoint], speed * Time.deltaTime * 10);
                else
                    CheckDir();
            }
        }
    }
    void ReverseEnum()
    {
        switch (direction)
        {
            case Direction.forward:
                direction = Direction.backward;
                break;
            case Direction.backward:
                direction = Direction.forward;
                break;
            default:
                break;
        }
    }
    void CheckDir()
    {
        if (curWayPoint == waypoints.Count - 1)
            direction = Direction.backward;
        if (curWayPoint == 0)
            direction = Direction.forward;

        curWayPoint += (int)direction;
    }
}