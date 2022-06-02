using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float camMoveSpeed = 5;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float airMoveAmount = 40;
    [SerializeField] private Camera cam;
    [HideInInspector] public Rigidbody rb;
    private float x, z;
    float xBound, yBound, zBound;
    public static float camX, camY;

    bool airborne = false;

    Vector3 lastDir;
    Vector3 moveDir;
    Vector3 lerpDir;
    Vector3 yVel;
    Vector3 groundNormal;
    int layerMask = 1 << 2;
    public bool grappling = false;
    bool isGrappling = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!Player.Instance.canMove)
            return;
        PlayerInput();
        CamMovement();
        Movement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void PlayerInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        camX += Input.GetAxis("Mouse X") * camMoveSpeed;
        camY += Input.GetAxis("Mouse Y") * camMoveSpeed;
        camY = Mathf.Clamp(camY, -90, 90);
    }
    void Movement()
    {
        moveDir = x * transform.right + z * transform.forward;
        moveDir = Vector3.ProjectOnPlane(moveDir, groundNormal);
        moveDir.Normalize();
        moveDir *= moveSpeed;

        if(!Grounded() && grappling)
        {
            if (!isGrappling)
            {
                Vector3 cVel = controller.velocity;
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                controller.enabled = false;
                rb.velocity = cVel;
                isGrappling = true;
            }

            lastDir = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            yVel.y = rb.velocity.y;
        }
        else
        {
            if (isGrappling)
            {
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rb.isKinematic = true;
                controller.enabled = true;
                isGrappling = false;
            }
        }
        if (!controller.enabled)
            return;

        if (lastDir != Vector3.zero)
            lerpDir = new Vector3(lastDir.x, 0, lastDir.z);
        if (Grounded())
        {
            lastDir = moveDir;
            if(moveDir.magnitude == 0)
            {
                lerpDir = Vector3.Lerp(lerpDir, Vector3.zero, Time.deltaTime * 10);
                controller.Move(lerpDir * Time.deltaTime);
            }

            if (!airborne)
            {
                yVel.y = 0;
            }
            if (Input.GetButtonDown("Jump"))
            {
                airborne = true;
                yVel.y = jumpHeight;
                lastDir += controller.velocity;
                Invoke("ResetJump", 0.4f);
            }
        }
        else
        {
            if(moveDir.magnitude > 0)
            {
                lastDir += moveDir * Time.deltaTime * airMoveAmount;

                if(lastDir.magnitude > maxSpeed)
                    lastDir *= maxSpeed / lastDir.magnitude;
            }

            xBound = controller.velocity.x <= 0 ? controller.velocity.x : -controller.velocity.x;
            yBound = controller.velocity.y <= 0 ? controller.velocity.y : -controller.velocity.y;
            zBound = controller.velocity.z <= 0 ? controller.velocity.z : -controller.velocity.z;

            lastDir = new Vector3(Mathf.Clamp(lastDir.x, xBound, -xBound), 0, Mathf.Clamp(lastDir.z, zBound, -zBound));
            if (controller.velocity.y == 0)
                yVel.y = 0;
            //yVel.y = Mathf.Clamp(yVel.y, yBound, -yBound);
            yVel.y -= 15 * Time.deltaTime;

            moveDir = lastDir + moveDir * Time.deltaTime;
        }
        moveDir += yVel;
        controller.Move(moveDir * Time.deltaTime);
    }
    public void AddForce(Vector3 force)
    {
        airborne = true;
        yVel.y = force.y;
        Invoke("ResetJump", 0.4f);
    }
    void CamMovement()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, camX, 0));
        cam.transform.rotation = Quaternion.Euler(new Vector3(-camY, camX, 0));
    }
    void ResetJump()
    {
        airborne = false;
    }
    public bool Grounded()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.35f, Vector3.down, out hit, 0.85f, ~layerMask))
        {
            groundNormal = hit.normal;
            return true;
        }
        else
            groundNormal = Vector3.up;
        return false;
    }
}