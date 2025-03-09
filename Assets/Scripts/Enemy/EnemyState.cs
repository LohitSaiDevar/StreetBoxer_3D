using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyStateID
{
    Idle,
    Chase,
    Attack
}

public interface IEnemyState
{
    EnemyStateID GetID();
    void Enter(EnemyAI enemy);
    void Update(EnemyAI enemy);
    void Exit(EnemyAI enemy);
}
