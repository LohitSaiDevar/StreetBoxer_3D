using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    public EnemyStateID GetID()
    {
        return EnemyStateID.Attack;
    }

    public void Enter(EnemyAI enemy)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(EnemyAI enemy)
    {
        throw new System.NotImplementedException();
    }

    public void Update(EnemyAI enemy)
    {
        throw new System.NotImplementedException();
    }
}
