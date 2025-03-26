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

    public void RayHitsPlayer()
    {
        enemy.isPlayerHit = true;
    }
    public void StartParryWindow()
    {
        enemy.isAttacking = true;
        Debug.Log("Parry window opened! isAttacking = " + enemy.isAttacking);
    }

    public void EndParryWindow()
    {
        enemy.isAttacking = false;
        enemy.playerIsParrying = false;
    }
}
