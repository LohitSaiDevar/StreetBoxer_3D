using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "Data/EnemyAttackData")]
public class EnemyData : ScriptableObject
{
    public float attackRange = 2f; // Range within which the enemy will attack
    public float attackSpeed = 1;
    public float attackDelay = 0.7f;
}
