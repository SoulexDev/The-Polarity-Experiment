using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutPlayerController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float camMoveSpeed = 5;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float airMoveAmount = 40;
    private Transform cam;

    private float x, z;
    float xBound, yBound, zBound;
    float camX, camY;

    bool airborne = false;

    Vector3 lastDir;
    Vector3 moveDir;
    Vector3 yVel;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        PlayerInput();
        Movement();
        CamMovement();
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
        moveDir.Normalize();
        moveDir *= moveSpeed;

        if (Grounded())
        {
            lastDir = moveDir;
            if (!airborne)
            {
                yVel.y = 0;
            }
            if (Input.GetButtonDown("Jump"))
            {
                airborne = true;
                yVel.y = jumpHeight;
                Invoke("ResetJump", 0.4f);
            }
        }
        else
        {
            if (moveDir.magnitude > 0)
            {
                lastDir += moveDir * Time.deltaTime * airMoveAmount;

                if (lastDir.magnitude > maxSpeed)
                    lastDir *= maxSpeed / lastDir.magnitude;
            }

            xBound = controller.velocity.x <= 0 ? controller.velocity.x : -controller.velocity.x;
            yBound = controller.velocity.y <= 0 ? controller.velocity.y : -controller.velocity.y;
            zBound = controller.velocity.z <= 0 ? controller.velocity.z : -controller.velocity.z;

            lastDir = new Vector3(Mathf.Clamp(lastDir.x, xBound, -xBound), 0, Mathf.Clamp(lastDir.z, zBound, -zBound));
            if (controller.velocity.y == 0)
                yVel.y = 0;
            yVel.y -= 15 * Time.deltaTime;

            moveDir = lastDir + moveDir * Time.deltaTime;
        }
        moveDir += yVel;
        controller.Move(moveDir * Time.deltaTime);
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
        return Physics.Raycast(transform.position, Vector3.down, 1.2f);
    }
}