using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHand : MonoBehaviour
{
    [Header("Hit CoolDown Settings")]
    [SerializeField] float hitCooldown = 0.5f; // Cooldown time in seconds
    [SerializeField] float lastHitTime = 0;

    EnemyController enemy;
    PlayerController player;

    int punchHeadCount;
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

    void AttackPlayer(Collider other)
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
                PlayerUIManager.Instance.currentStamina -= 10;
                PlayerUIManager.Instance.staminaBar.UpdateStaminaBar(PlayerUIManager.Instance.currentStamina);
            }
        }
    }
}
