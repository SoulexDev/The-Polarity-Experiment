using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public float rotationAmount = 1;
    public PlayerController controller;

    float defaultPosY = 0;
    float defaultRotZ = 0;
    float timer = 0;
    public float rotMult = 0.2f;
    public bool handObj = false;
    Quaternion defaultRot;

    void Start()
    {
        defaultPosY = transform.localPosition.y;
        defaultRotZ = transform.localRotation.eulerAngles.z;
        defaultRot = transform.localRotation;
    }

    void Update()
    {
        Vector3 moveDirection = controller.controller.velocity;
        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * walkingBobbingSpeed;
            if (!handObj)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer * (moveDirection.magnitude / 5)) * bobbingAmount, transform.localPosition.z), Time.deltaTime * 8);
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, defaultRotZ + Mathf.Sin(timer * rotMult) * rotationAmount * 15 * (moveDirection.magnitude / 5));
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer * (moveDirection.magnitude / 5)) * bobbingAmount * (moveDirection.magnitude / 5), transform.localPosition.z), Time.deltaTime * 8);
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, defaultRotZ + Mathf.Sin(timer * rotMult) * rotationAmount * 15 * (moveDirection.magnitude / 5));
            }
        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z), Time.deltaTime * 5);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultRot, Time.deltaTime * 5);
        }
    }
}