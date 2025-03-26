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
    [SerializeField] float firstAttackDelay;
    public bool isAttacking;
    bool isPlayerAttacking;
    float numberOfHitsTaken = 0;
    [SerializeField] float numberOfHits;
    bool isTooManyHits;
    public bool isPlayerHit;
    [SerializeField] GameObject cam;
    //StateMachine
    EnemyStateMachine stateMachine;
    [SerializeField] EnemyStateID initialState;

    public bool playerIsParrying;

    bool readyToAttack = true;
    bool hasAttacked = false;

    AttackMove playerAttackMove;
    private void Start()
    {
        stateMachine = new EnemyStateMachine(this);
        stateMachine.RegisterState(new EnemyChaseState());
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (readyToAttack)
        {
            Invoke(nameof(Attack), attackDelay);
        }
    }
    public void TakeDamage()
    {
        if (numberOfHitsTaken >= numberOfHits)
        {
            isTooManyHits = true;
            if (isTooManyHits)
            {
                Block();
                isPlayerAttacking = false;
            }
        }
        else if (playerIsParrying)
        {
            animator.SetTrigger("Crit");
        }
        else
        {
            isPlayerAttacking = true;
            switch (playerAttackMove)
            {
                case AttackMove.Cross:

                    float randomChance_1 = Random.Range(0, 3);
                    if (randomChance_1 == 0)
                    {
                        animator.SetTrigger("HeadHit_1");
                        Debug.Log("HeadHit_1");
                    }
                    else if (randomChance_1 == 1)
                    {
                        animator.SetTrigger("HeadHit_2");
                        Debug.Log("HeadHit_2");
                    }
                    else if (randomChance_1 == 2)
                    {
                        animator.SetTrigger("HeadHit_3");
                        Debug.Log("HeadHit_3");
                    }
                    break;

                case AttackMove.Jab:
                    animator.SetTrigger("SideHit");
                    Debug.Log("SideHit");
                    break;

                case AttackMove.Hook:
                    float randomChance_3 = Random.Range(0, 2);
                    if (randomChance_3 == 0)
                    {
                        animator.SetTrigger("HitToBody_1");
                        Debug.Log("HitToBody_1");
                    }
                    else if (randomChance_3 == 1)
                    {
                        animator.SetTrigger("HitToBody_2");
                        Debug.Log("HitToBody_2");
                    }
                    break;
            }
            numberOfHitsTaken++;
        }
            
        //Debug.Log($"Collider: {collider.gameObject.name}");
    }

    void AttackRaycast()
    {
        Ray ray = new Ray(attackPoint.position, attackPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance, attackLayer))
        {
            Debug.Log("Ray is hit");
            PlayerController player = hit.collider.GetComponent<PlayerController>();
            player.TakeDamage(40);
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
            if (!player.isEnemyHit && !isPlayerAttacking)
            {
                animator.SetTrigger("Punch");
                if (!playerIsParrying && isPlayerHit)
                {
                    Debug.Log("Does work");
                    AttackRaycast();
                    isPlayerHit = false;
                }
            }
        }
    }

    public void ParryAttempt()
    {
        if (isAttacking)
        {
            Debug.Log("Parried");
            animator.SetTrigger("isParried");
            playerIsParrying = true;
            isAttacking = false;
        }
    }
    void Block()
    {
        animator.SetTrigger("Block");
        isTooManyHits = false;
    }
}
