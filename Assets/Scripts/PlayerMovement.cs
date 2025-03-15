using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    Vector2 input;
    Vector3 currentVelocity;
    [SerializeField] float acceleration;
    [SerializeField] float moveSpeed;
    float velocityXSmoothing;
    float velocityZSmoothing;
    [SerializeField] Transform playerMesh;
    //[SerializeField] float smoothTransitionTime = 0.1f;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 move = new Vector3(input.x, 0f, input.y) * moveSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, move, acceleration * Time.deltaTime);
        Debug.Log($"Current velocity:  {currentVelocity}, Input: {input} ");
        animator.SetFloat("VelocityX", currentVelocity.x);
        animator.SetFloat("VelocityZ", currentVelocity.z);
    }

    private void LateUpdate()
    {
        transform.position = playerMesh.position;
        playerMesh.localPosition = Vector3.zero;
    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
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
