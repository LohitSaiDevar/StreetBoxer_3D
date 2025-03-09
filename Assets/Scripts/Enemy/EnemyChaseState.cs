using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyChaseState : IEnemyState
{
    NavMeshAgent agent;
    float speed;
    public EnemyStateID GetID()
    {
        return EnemyStateID.Chase;
    }
    public void Enter(EnemyAI enemy)
    {
        agent = enemy.GetComponent<NavMeshAgent>();

    }

    public void Exit(EnemyAI enemy)
    {
        
    }

    public void Update(EnemyAI enemy)
    {
        agent.SetDestination(enemy.target.position);
        speed = Mathf.Lerp(speed, agent.velocity.magnitude, Time.deltaTime * 5);
        enemy.animator.SetFloat("Velocity", speed);
        //Debug.Log($"velocity: {agent.velocity}");
    }
}
