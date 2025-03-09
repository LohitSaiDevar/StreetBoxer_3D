using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform player;
    [SerializeField] Transform attackPoint;
    [SerializeField] Vector3 offset = new Vector3(0, 0, 0); // Adjust for positioning
    [SerializeField] float smoothSpeed = 10f;
    [SerializeField] Vector3 headPosition;


    [Header("Camera Movement Settings")]
    [SerializeField] float sensitivityX = 2f; // Mouse/Gamepad sensitivity
    [SerializeField] float sensitivityY = 2f;
    [SerializeField] float maxLookAngleTop = 80f; // Prevents looking too far up
    [SerializeField] float maxLookAngleBottom = 80f; // Prevents looking too far down
    [SerializeField] float playerRotationSpeed = 10f; // Smooth rotation speed for player

    private Vector2 lookInput;
    private float pitch = 0f; // Up/down rotation
    private float yaw = 0f; // Left/right rotation
    void LateUpdate()
    {
        if (head == null || player == null) return;

        Vector3 targetPosition = head.position + offset;

        // Directly match head position with offset
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        head.localPosition += headPosition; // Move slightly backward
        // Directly match head rotation
        //transform.rotation = Quaternion.Lerp(transform.rotation, head.rotation, smoothSpeed * Time.deltaTime);

        RotateCamera();

    }

    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void RotateCamera()
    {
        //  Horizontal Rotation (Yaw - Turning Left/Right)
        yaw += lookInput.x * sensitivityX;

        //  Apply smooth player rotation
        Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);

        //  Vertical Rotation (Pitch - Looking Up/Down)
        pitch -= lookInput.y * sensitivityY;
        pitch = Mathf.Clamp(pitch, -maxLookAngleTop, maxLookAngleBottom); // Prevent looking too far up/down

        //  Apply Rotation to Camera (Only Pitch)
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
