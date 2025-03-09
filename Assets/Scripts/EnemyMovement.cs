using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyController enemyController;
    public Transform player; // Assign the player transform in the inspector
    Animator animator;
    public float velocityZ; // Adjust this value for desired enemy walking speed
    public float acceleration;
    public float stopDistance = 2.5f; // Adjust this value to control how close the enemy gets to the player
    [SerializeField] float maxVelocity;
    [SerializeField] float minX = -4.3f;
    [SerializeField] float maxX = 4.3f;
    [SerializeField] float minZ = -4.3f;
    [SerializeField] float maxZ = 4.3f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (!enemyController.isDefeated)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;
            // Look at the player
            transform.LookAt(player);

            // Calculate distance to player
            float distance = directionToPlayer.magnitude;

            if (distance < stopDistance)
            {
                transform.position -= directionToPlayer.normalized * Time.deltaTime * acceleration;
                velocityZ -= Time.deltaTime * acceleration;
                animator.SetFloat("VelocityZ", velocityZ);
                return;
            }

            Vector3 newPosition = transform.position + directionToPlayer.normalized * Time.deltaTime * velocityZ;

            // Clamp the new position within the boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            // Apply the clamped position
            transform.position = newPosition;
            // Move towards player if not in stop distance
            if (velocityZ < maxVelocity)
            {
                velocityZ += Time.deltaTime * acceleration;
            }
            animator.SetFloat("VelocityZ", velocityZ);
        }
    }
}

