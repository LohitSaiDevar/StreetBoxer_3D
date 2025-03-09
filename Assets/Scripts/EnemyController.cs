using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    [SerializeField] float minAttackInterval = 1f;
    [SerializeField] float maxAttackInterval = 3f;

    float dodgeWarningTime = 0.5f;
    Coroutine attackCoroutine;
    internal bool isDefeated;
    private void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void BodyHit()
    {
        StopAttack();
        anim.SetTrigger("BodyHit");
        EnemyUIManager.Instance.currentStamina -= 10;
        EnemyUIManager.Instance.currentHealth -= 5;
        StartCoroutine(EnemyUIManager.Instance.SmoothHealthBarTransition(EnemyUIManager.Instance.currentHealth));
        StartCoroutine(EnemyUIManager.Instance.SmoothStaminaBarTransition(EnemyUIManager.Instance.currentStamina));
    }
    public void HeadHit()
    {
        StopAttack();
        anim.SetTrigger("LeftSideHeadHit");
        EnemyUIManager.Instance.currentStamina -= 20;
        EnemyUIManager.Instance.currentHealth -= 7.5f;
        StartCoroutine(EnemyUIManager.Instance.SmoothHealthBarTransition(EnemyUIManager.Instance.currentHealth));
        StartCoroutine(EnemyUIManager.Instance.SmoothStaminaBarTransition(EnemyUIManager.Instance.currentStamina));
    }
    public void DizzyEffect()
    {
        anim.SetTrigger("PushedBack");
    }

    public void KO()
    {
        anim.SetTrigger("KO");
        isDefeated = true;
        if (EnemyUIManager.Instance.currentHealth > 0)
        {
            StartCoroutine(KoSetTimer());
        }
        else if (EnemyUIManager.Instance.currentHealth <= 0)
        {
            GameManager.Instance.isGameOver = true;
            GameManager.Instance.isWinner = true;
            GameManager.Instance.GameOver();
        }
    }
    void GettingUp()
    {
        anim.SetTrigger("GettingUp");
    }
    internal void Jab()
    {
        anim.SetTrigger("Jab");
    }

    internal void Cross()
    {
        anim.SetTrigger("Cross");
    }

    internal void UpperCut()
    {
        anim.SetTrigger("UpperCut");
    }

    internal void Dodge()
    {
        StopAttack();
        anim.SetTrigger("Dodge");
    }
    internal void StopAttack()
    {
        anim.ResetTrigger("Jab");
        anim.ResetTrigger("Cross");
    }
    IEnumerator AttackPlayer()
    {
        while (true)
        {
            float waitTime = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(waitTime - dodgeWarningTime);

            Debug.Log("Dodge!");

            yield return new WaitForSeconds(dodgeWarningTime);
            int attackChoice = Random.Range(0, 2);

            switch (attackChoice)
            {
                case 0:
                    Jab();
                    break;
                case 1:
                    Cross();
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isDefeated)
        {
            if (other.CompareTag("PlayerDetector"))
            {
                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(AttackPlayer());
                }
            }
        }
    }

    // Detect when the enemy exits the player's trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerDetector"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    public IEnumerator KoSetTimer()
    {
        yield return new WaitForSeconds(5);
        GettingUp();
        EnemyUIManager.Instance.currentStamina = EnemyUIManager.Instance.maxStamina;
        StartCoroutine(EnemyUIManager.Instance.SmoothStaminaBarTransition(EnemyUIManager.Instance.maxStamina));

        // Wait for the getting up animation to finish (assuming it's 2 seconds here)
        yield return new WaitForSeconds(9);

        isDefeated = false;
    }

    
}
