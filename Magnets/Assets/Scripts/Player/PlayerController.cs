using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float camMoveSpeed = 5;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private Camera cam;
    private float x, z;
    private float camX, camY;

    bool jumping = false;

    Vector3 lastDir;
    Vector3 moveDir;
    Vector3 yVel;
    int layerMask = 1 << 2;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        PlayerInput();
        CamMovement();
        Movement();
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
            if (!jumping)
            {
                yVel.y = 0;
            }
            if (Input.GetButtonDown("Jump"))
            {
                jumping = true;
                yVel.y = jumpHeight;
                Invoke("ResetJump", 0.4f);
            }
        }
        else
        {
            yVel.y -= 18 * Time.deltaTime;
            moveDir = lastDir + (moveDir * 0.3f);
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
        jumping = false;
    }
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.2f, ~layerMask);
    }
}