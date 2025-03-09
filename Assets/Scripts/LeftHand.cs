using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeftHand : MonoBehaviour
{
    [Header("Hit CoolDown Settings")]
    [SerializeField] float hitCooldown = 0.5f; // Cooldown time in seconds
    [SerializeField] float lastHitTime = 0;

    EnemyController enemy;
    PlayerController player;
    private void Start()
    {
        if (this.gameObject.CompareTag("EnemyHand"))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        else if (this.gameObject.CompareTag("PlayerHand"))
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        }
    }
    int punchHeadCount;
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastHitTime < hitCooldown) return;

        lastHitTime = Time.time;
        
        if (this.gameObject.CompareTag("EnemyHand"))
        {
            AttackPlayer(other);
        }
        else if (this.gameObject.CompareTag("PlayerHand"))
        {
            AttackEnemy(other);
        }
    }

    void AttackEnemy(Collider other)
    {
        if (this.gameObject.CompareTag("PlayerHand"))
        {
            if (other.CompareTag("Head"))
            {
                enemy.HeadHit();
                punchHeadCount++;
                if (punchHeadCount >= 3)
                {
                    enemy.DizzyEffect();
                    punchHeadCount = 0;
                }
            }
            else if (other.CompareTag("Body"))
            {
                enemy.BodyHit();
            }
        }
    }

    void AttackPlayer(Collider other)
    {
        if (this.gameObject.CompareTag("EnemyHand"))
        {
            if (other.CompareTag("PlayerHead"))
            {
                if (!player.isDefeated)
                {
                    player.TakingHit();
                }
            }
            else if (other.CompareTag("PlayerBody"))
            {
                if (!player.isDefeated)
                {
                    player.TakingHit();
                }
            }
            else if (other.CompareTag("Block"))
            {
                if (!player.isDefeated)
                {
                    Debug.Log("Hit Arm!");
                    PlayerUIManager.Instance.currentStamina -= 10;
                    PlayerUIManager.Instance.staminaBar.UpdateStaminaBar(PlayerUIManager.Instance.currentStamina);
                }
            }
        }
    }
}
