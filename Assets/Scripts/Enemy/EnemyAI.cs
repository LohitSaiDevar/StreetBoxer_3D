using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public Transform target;
    [SerializeField] Transform attackPoint;
    [SerializeField] EnemyData enemyAttackData;
    [SerializeField] CharacterStats enemyStats;
    //bool readyToAttack = true;
    //bool attacking = false;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float attackRadius;
    [SerializeField] float attackDistance;
    [SerializeField] float attackDelay;
    public bool isAttacking;
    
    bool isPlayerHit;
    [SerializeField] GameObject cam;
    //StateMachine
    EnemyStateMachine stateMachine;
    [SerializeField] EnemyStateID initialState;
    private void Start()
    {
        stateMachine = new EnemyStateMachine(this);
        stateMachine.RegisterState(new EnemyChaseState());
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.Update();
        StartCoroutine(AttackDelay());
    }
    public void TakeDamage()
    {
        animator.SetTrigger("IsHit");
        //Debug.Log($"Collider: {collider.gameObject.name}");
    }

    void AttackRaycast()
    {
        Ray ray = new Ray(attackPoint.position, attackPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance, attackLayer))
        {
            Debug.Log("Ray is hit");
            //Animator camAnimator = cam.GetComponent<Animator>();
            //camAnimator.SetTrigger("isHit");
        }

        Debug.DrawRay(attackPoint.position, attackPoint.forward * attackDistance, Color.red, 0.1f);
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * attackDistance);
        }
    }

    void Attack()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < attackDistance)
        {
            PlayerController player = target.GetComponentInParent<PlayerController>();
            if (!player.isEnemyHit)
            {
                animator.SetTrigger("Punch");
                if (isAttacking && !player.CanParry())
                {
                    AttackRaycast();
                    isAttacking = false;
                }
                else if(isAttacking && player.CanParry())
                {
                    Debug.Log("Parried");
                    animator.SetTrigger("isParried");
                    isAttacking = false;
                }
            }
        }
    }

    IEnumerator AttackDelay()
    {
        Attack();
        yield return new WaitForSeconds(attackDelay);
    }
}
