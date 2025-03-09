using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 500;
    float xRotation;
    float yRotation;
    Vector3 camInput;
    [SerializeField] float topClamp = -90;
    [SerializeField] float bottomClamp = 90;

    [SerializeField] Camera cam;
    PlayerInput playerInput;
    Vector3 camPositionOffset;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = camInput.x;
        float mouseY = camInput.y;

        xRotation -= mouseY * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation += mouseX * mouseSensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
    public void Look(InputAction.CallbackContext context)
    {
        camInput = context.ReadValue<Vector2>();
        //Debug.Log(camInput);
    }

}
