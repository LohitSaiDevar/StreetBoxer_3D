using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public EnemyStateID GetID()
    {
        return EnemyStateID.Idle;
    }
    public void Enter(EnemyAI enemy)
    {
        enemy.animator.SetFloat("Velocity", 0);
    }
    public void Exit(EnemyAI enemy)
    {
        
    }
    public void Update(EnemyAI enemy)
    {
        
    }
}
