using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger_Enemy : MonoBehaviour
{
    [SerializeField] EnemyAI enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationTrigger()
    {
        enemy.isAttacking = true;
    }
}
