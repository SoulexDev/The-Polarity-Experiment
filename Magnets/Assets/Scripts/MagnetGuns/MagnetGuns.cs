using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetGuns : MonoBehaviour
{
    [SerializeField] private string mouseInput = "Fire1";
    public Transform magnetBone;
    public Vector3 pointPos;
    [SerializeField] private Transform cableBone;
    [SerializeField] private Transform gunBone;
    [SerializeField] private Transform cam;
    [SerializeField] private MagneticObject.Pole pole;
    [SerializeField] private AudioClip[] ropeClips;
    [SerializeField] private AudioClip[] magnetClips;

    [SerializeField] private LayerMask ropeMask;
    private AudioSource source;
    private TubeRenderer cable;

    //Cable
    [SerializeField] private List<EdgePoint> points = new List<EdgePoint>();
    public int curPoint = 0;

    bool extended = false;
    bool retracting = false;
    bool connected = false;
    bool letGo = false;
    bool dontDestroy = false;

    Vector3 gPoint;
    Quaternion gRot;
    Vector3 ogPos;

    RaycastHit hit;

    [HideInInspector] public MagneticObject conMag;
    int layerMask = 1 << 2;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        cable = GetComponent<TubeRenderer>();
        EdgePoint magPoint = new EdgePoint();
        EdgePoint gunPoint = new EdgePoint();
        magPoint.position = magnetBone.position;
        gunPoint.position = gunBone.position;
        points.Add(magPoint);
        points.Add(gunPoint);
    }
    void Start()
    {
        ogPos = magnetBone.localPosition;
        cam = Camera.main.transform;
        layerMask = ~layerMask;
    }

    void Update()
    {
        if (!Player.Instance.canMove)
            return;
        PlayerInput();
        UpdateExtensions();

        if(magnetBone.parent == null && !dontDestroy)
        {
            DontDestroyOnLoad(magnetBone.gameObject);
            dontDestroy = true;
        }
        else if(magnetBone.parent != null)
        {
            dontDestroy = false;
        }
    }
    private void LateUpdate()
    {
        UpdateCables();
    }
    void UpdateCables()
    {
        RaycastHit hit;
        RaycastHit hit2;
        points[0].position = magnetBone.position;
        points[points.Count - 1].position = gunBone.position;
        Vector3 dir = (points[points.Count - 1].position - points[curPoint].position).normalized;
        float curDot = Vector3.Dot(points[curPoint].normal, dir);

        //if(Physics.Linecast(points[points.Count - 1], points[curPoint], out hit, ropeMask))
        //{
        //    if (hit.distance < 0.2f)
        //        return;
        //}

        if (Physics.Linecast(points[points.Count - 1].position, points[curPoint].position, out hit, ropeMask))
        {
            Vector3 point = hit.point;
            Vector3 outDir = hit.normal;

            if (Physics.Raycast(points[curPoint].position, dir, out hit2, ropeMask))
            {
                Vector3 trigDir = hit2.point - hit.point;
                outDir = hit.normal + hit2.normal;
                outDir *= 0.1f;
                point = Vector3.ProjectOnPlane(trigDir, hit.normal) + hit.point + outDir;
            }
            curPoint++;
            points[curPoint].dot = Vector3.Dot(hit2.normal, dir);
            points[curPoint].normal = hit2.normal;
            EdgePoint edgePoint = new EdgePoint();
            edgePoint.position = point;
            points.Insert(curPoint, edgePoint);
        }
        //if(curPoint > 0 && points[curPoint].dot > curDot)
        //{
        //    points.RemoveAt(curPoint);
        //    curPoint--;
        //}
        if (curPoint > 0 && !Physics.Linecast(points[curPoint - 1].position, points[points.Count - 1].position, ropeMask))
        {
            Vector3 preDir = (points[curPoint - 1].position - points[points.Count - 1].position).normalized;
            float angle = Vector3.Angle(dir, preDir);
            if (angle < 10 || angle > 170)
            {
                points.RemoveAt(curPoint);
                curPoint--;
            }
        }
        pointPos = points[curPoint].position;
        Vector3[] pointsPos = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            pointsPos[i] = points[i].position;
        }
        cable.SetPositions(pointsPos);
    }
    void PlayerInput()
    {
        if (conMag != null && !conMag.connected)
        {
            extended = false;
            conMag = null;
            letGo = false;
            connected = false;
        }
        if (Input.GetButtonDown(mouseInput))
        {
            if (connected)
                letGo = true;
            if (extended || retracting)
                return;
            gPoint = GetRayPoint();
            StartCoroutine(ExtendMagnet(gPoint));
        }
        if(Input.GetButton(mouseInput) && connected)
        {
            if (Vector3.Distance(magnetBone.position, gunBone.TransformPoint(ogPos)) > 0.5f)
            {
                ExecuteConnected();
                if(!source.isPlaying)
                    source.PlayOneShot(ropeClips[1]);
            }
            else
            {
                if (retracting)
                {
                    source.Stop();
                    source.PlayOneShot(magnetClips[1]);
                }
                ConnectMagnet();
            }
        }
        else if(connected)
        {
            StopExecuteConnected();
            source.Stop();
        }
        if (Input.GetButtonUp(mouseInput) && connected && letGo)
        {
            RetractMagnet();
        }
    }
    void UpdateExtensions()
    {
        Quaternion desiredRot;
        if(Vector3.Distance(magnetBone.position, gunBone.TransformPoint(ogPos)) > 0.1f)
        {
            desiredRot = Quaternion.LookRotation(magnetBone.position - gunBone.position, Vector3.up) * Quaternion.Euler(new Vector3(-90, 0, 180));
        }
        else
        {
            desiredRot = cam.rotation * Quaternion.Euler(new Vector3(-90, 0, 180));
        }

        if (extended)
        {
            if(magnetBone.parent == null)
                magnetBone.rotation = Quaternion.Slerp(magnetBone.rotation, gRot, Time.deltaTime * 10);
        }
        else
        {
            if (Vector3.Distance(magnetBone.position, gunBone.TransformPoint(ogPos)) > 0.15f)
            {
                Vector3 travelPos = curPoint == 0 ? gunBone.TransformPoint(ogPos) : pointPos;

                magnetBone.position = Vector3.MoveTowards(magnetBone.position, travelPos, 0.35f * Time.deltaTime * 60);

                if(curPoint > 0 && Vector3.Distance(magnetBone.position, travelPos) < 1)
                {
                    points.RemoveAt(curPoint);
                    curPoint--;
                }

                if(!source.isPlaying)
                    source.PlayOneShot(ropeClips[1]);
            }
            else
            {
                magnetBone.SetParent(gunBone);
                magnetBone.localPosition = ogPos;
                if (retracting)
                {
                    source.Stop();
                    source.PlayOneShot(magnetClips[1]);
                }
                retracting = false;
            }
        }
        gunBone.rotation = Quaternion.Slerp(gunBone.rotation, desiredRot, Time.deltaTime * 15);
        cableBone.rotation = gunBone.rotation;

        if(!extended)
            magnetBone.rotation = gunBone.rotation;
    }
    IEnumerator ExtendMagnet(Vector3 hitPoint)
    {
        extended = true;
        magnetBone.SetParent(null);

        source.PlayOneShot(magnetClips[0]);
        source.PlayOneShot(ropeClips[0]);

        while (Vector3.Distance(magnetBone.position, hitPoint) > 0.5f)
        {
            magnetBone.position = Vector3.MoveTowards(magnetBone.position, hitPoint, 0.5f * Time.deltaTime * 60);
            yield return null;
        }
        retracting = true;

        if (hit.collider != null && hit.collider.TryGetComponent(out MagneticObject magneticObject))
        {
            if (magneticObject.connection == null && (magneticObject.pole != pole))
            {
                conMag = magneticObject;
                magnetBone.rotation = gRot;
                conMag.Connect(this, gunBone, magnetBone, pole);
                connected = true;
                source.Stop();
                StopAllCoroutines();
            }
        }
        yield return new WaitForSeconds(0.1f);
            source.Stop();
        yield return new WaitForSeconds(0.1f);
            extended = false;
    }
    void ExecuteConnected()
    {
        if (conMag != null)
            conMag.Execute();
    }
    void StopExecuteConnected()
    {
        if(conMag != null)
            conMag.StopExecute();
    }
    Vector3 GetRayPoint()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out hit, 15, ~LayerMask.GetMask("PlayerCollision", "Ignore Raycast")))
        {
            gRot = Quaternion.LookRotation(hit.normal, Vector3.up) * Quaternion.Euler(new Vector3(-90, 0, 0));
            return hit.point;
        }
        else
        {
            gRot = gunBone.transform.rotation;
            return ray.GetPoint(15);
        }
    }
    public void ConnectMagnet()
    {
        //source.Stop();
        extended = false;
        retracting = false;
        connected = false;

        if(conMag != null)
        {
            conMag.Disconnect();
            conMag = null;
        }

        magnetBone.position = gunBone.TransformPoint(ogPos);
        magnetBone.SetParent(gunBone);
        magnetBone.localPosition = ogPos;
    }
    public void RetractMagnet()
    {
        if (magnetBone.parent == null)
            return;
        extended = false;
        if(conMag != null)
            conMag.Disconnect();
        conMag = null;
        letGo = false;
        connected = false;
    }
}
public class EdgePoint
{
    public Vector3 position;
    public Vector3 normal;
    public float dot;
}