using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackMove
{
    Cross,
    Jab,
    Hook,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Attack System")]
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackDelay = 0.4f;
    //[SerializeField] float attackSpeed = 1;
    //[SerializeField] float attackDamage = 1;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackCooldownTime = 1;
    bool readyToAttack = true;
    bool attacking = false;
    public bool isEnemyHit;
    bool isEnemyStunned;
    int attackCount = 0;
    internal bool isDefeated;
    AttackMove attackMove;

    public static Action OnPlayerAttack;

    [Header("Sound Effects")]
    AudioSource audioSource;
    [SerializeField] AudioClip handWaveSound;
    [SerializeField] AudioClip punchSound;
    [SerializeField] Camera cam;


    [Header("Animations")]
    string currentAnimationState;
    [SerializeField] float transitionDuration = 0.2f;

    public const string ATTACK1 = "Cross";
    public const string ATTACK2 = "Jab";
    public const string ATTACK3 = "Hook";

    [Header("Parry System")]
    public float parryWindow;
    public bool isParrying;
    bool isParryAvailable;

    [SerializeField] EnemyAI enemy;
    void Awake()
    {
        if (audioSource != null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            //AttemptToGetUp();
        }
    }

    #region Attack
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!readyToAttack || attacking) return;

            readyToAttack = false;
            attacking = true;
            //Debug.Log("attack");
            Invoke(nameof(ResetAttack), attackDelay);

            AudioManager.Instance.PlaySFX(handWaveSound);

            if (attackCount == 0)
            {
                animator.SetTrigger(ATTACK1);
                attackMove = AttackMove.Cross;
                Debug.Log("AttackMove: " + attackMove);
                attackCount++;
            }
            else if (attackCount == 1)
            {
                animator.SetTrigger(ATTACK2);
                attackMove = AttackMove.Jab;
                Debug.Log("AttackMove: " + attackMove);
                attackCount++;
            }
            else if (attackCount == 2)
            {
                animator.SetTrigger(ATTACK3);
                attackMove = AttackMove.Hook;
                Debug.Log("AttackMove: " + attackMove);
                attackCount = 0;
            }
        }
    }

    
    void ResetAttack()
    {
        readyToAttack = true;
        isEnemyHit = false;
        attacking = false;
        isParrying = false;
    }
    public void AttackRaycast()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.transform.position, attackRange, attackLayer);

        foreach (Collider hit in hits)
        {
            EnemyAI enemy = hit.GetComponentInParent<EnemyAI>();
            enemy.TakeDamage();
            isEnemyHit = true;
            //Debug.Log("Enemy has been hit: " + hit.gameObject.name); // Optional debug
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
        }
    }

    public void TakeDamage(float damage)
    {
        cam.GetComponent<CameraShake>().ShakeCamera();
        AudioManager.Instance.PlaySFX(punchSound);
        Debug.Log("Damage dealt: " +  damage);
    }

    #endregion

    #region Parry
    public void Parry(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("Block");
            if (enemy != null)
            {
                //Debug.Log($"Parry Pressed - Enemy isAttacking: {enemy.isAttacking}"); // Debug log
                enemy.ParryAttempt();
            }
            else
            {
                //Debug.Log("Enemy reference is null!");
            }
        }
    }
    #endregion
    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(punchSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }

    public void ChangeAnimationState(string newState)
    {
        if(currentAnimationState == newState) return;

        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, transitionDuration);
    }

    public void TakingHit()
    {
        animator.SetTrigger("TakingHit");
        PlayerUIManager.Instance.currentStamina -= 10;
        PlayerUIManager.Instance.currentHealth -= 5;
        StartCoroutine(PlayerUIManager.Instance.SmoothStaminaBarTransition(PlayerUIManager.Instance.currentStamina));
        StartCoroutine(PlayerUIManager.Instance.SmoothHealthBarTransition(PlayerUIManager.Instance.currentHealth));
    }

    public void KO()
    {
        animator.SetTrigger("KO");
        isDefeated = true;
        if(PlayerUIManager.Instance.currentHealth <= 0)
        {
            GameManager.Instance.isGameOver = true;
            GameManager.Instance.isWinner = false;
            GameManager.Instance.GameOver();
        }

    }
    public void GettingUp()
    {
        animator.SetTrigger("GettingUp");
        PlayerUIManager.Instance.getUpBar.gameObject.SetActive(false);
        isDefeated = false;
        Debug.Log("Is defeated: " + isDefeated);
        
    }
    IEnumerator AttackCooldown()
    {
        readyToAttack = true;
        yield return new WaitForSeconds(attackCooldownTime);
        readyToAttack = false;
    }

    private void AttemptToGetUp()
    {
        if (PlayerUIManager.Instance.currentHealth > 0)
        {
            PlayerUIManager.Instance.gettingUpMeterValue = PlayerUIManager.Instance.currentHealth / 10;
        }

        float maxValue = PlayerUIManager.Instance.getUpBar.GetMaxValue();
        float currentValue = PlayerUIManager.Instance.getUpBar.GetCurrentValue();
        currentValue += PlayerUIManager.Instance.gettingUpMeterValue;
        PlayerUIManager.Instance.getUpBar.UpdateGetUpBar(currentValue);

        if (currentValue >= maxValue)
        {
            GettingUp();
            PlayerUIManager.Instance.staminaBar.UpdateStaminaBar(100);
        }
    }
}
