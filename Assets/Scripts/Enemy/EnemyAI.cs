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
        RaycastHit[] hits = Physics.RaycastAll(attackPoint.transform.position, attackPoint.transform.forward, attackDistance, attackLayer);
        if (hits.Length > 0)
        {
            RaycastHit closestHit = hits.OrderBy(h => h.distance).First();
            PlayerController player = closestHit.collider.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(enemyStats.attackPower);
                Debug.Log("Player has been hit: " + closestHit.collider.gameObject.name); // Optional debug
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
        }
    }

    void Attack()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < attackDistance)
        {
            if (!target.GetComponent<PlayerController>().isEnemyHit)
            {
                animator.SetTrigger("Punch");
                if (isAttacking)
                {
                    AttackRaycast();
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
