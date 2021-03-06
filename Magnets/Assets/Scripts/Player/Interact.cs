using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private IInteractable interactable;
    [SerializeField] private GameObject interactVisual;
    float holdCounter = 0;
    void Update()
    {
        PlayerInput();
    }
    void PlayerInput()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.collider.TryGetComponent(out IInteractable _interactable))
            {
                interactable = _interactable;
            }
            else
                interactable = null;
        }
        else
            interactable = null;

        if (Input.GetButtonDown("Interact"))
        {
            if(interactable != null)
            {
                interactable.Interact();
            }
        }
        if (Input.GetButton("Interact"))
        {
            holdCounter += Time.deltaTime;
            if (holdCounter >= 1 && interactable != null)
                interactable.Interact();
        }
        else
            holdCounter = 0;
        interactVisual.SetActive(interactable != null);
    }
}