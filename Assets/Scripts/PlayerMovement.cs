using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    Vector2 movementInput;
    Vector2 smoothVelocity;
    [SerializeField] float currentSpeed;
    float velocityXSmoothing;
    float velocityZSmoothing;
    [SerializeField] Transform playerMesh;
    [SerializeField] float smoothTransitionTime = 0.1f;
    private void Start()
    {

    }
    private void Update()
    {
        // Get normalized direction and raw magnitude
        Vector2 inputDirection = movementInput.normalized;
        float inputMagnitude = movementInput.magnitude;

        float targetSpeed = Mathf.Clamp01(inputMagnitude);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocityXSmoothing, smoothTransitionTime);

        Vector2 scaledVelocity = inputDirection * currentSpeed;

        animator.SetFloat("VelocityX", scaledVelocity.x);
        animator.SetFloat("VelocityZ", scaledVelocity.y);

        if (movementInput.x > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DodgeRight();
            }
        }
        
        if (movementInput.x < 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DodgeLeft();
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = playerMesh.position;
        playerMesh.localPosition = Vector3.zero;
    }
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    

    
    void DodgeLeft()
    {
        animator.SetTrigger("DodgeLeft");
    }

    void DodgeRight()
    {
        animator.SetTrigger("DodgeRight");
    }
}
