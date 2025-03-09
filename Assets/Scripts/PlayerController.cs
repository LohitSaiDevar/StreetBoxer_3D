using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Attack System")]
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackDelay = 0.4f;
    [SerializeField] float attackSpeed = 1;
    [SerializeField] float attackDamage = 1;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackCooldownTime = 1;
    bool readyToAttack = true;
    bool attacking = false;
    public bool isEnemyHit;
    int attackCount = 0;
    internal bool isDefeated;

    [Header("Sound Effects")]
    AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip handWaveSound;
    [SerializeField] Camera cam;


    [Header("Animations")]
    string currentAnimationState;
    [SerializeField] float transitionDuration = 0.2f;

    public const string ATTACK1 = "Cross";
    public const string ATTACK2 = "Jab";
    public const string ATTACK3 = "Hook";

    [Header("Parry System")]
    [SerializeField] float parryWindow;
    bool isParrying;
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
            AttemptToGetUp();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!readyToAttack || attacking) return;

            readyToAttack = false;
            attacking = true;
            //Debug.Log("attack");
            Invoke(nameof(ResetAttack), attackDelay);

            //audioSource.pitch = Random.Range(0.9f, 1.1f);
            //audioSource.PlayOneShot(handWaveSound);

            if (attackCount == 0)
            {
                animator.SetTrigger(ATTACK1);
                //Debug.Log("Attack 1");
                attackCount++;
            }
            else if(attackCount == 1)
            {
                animator.SetTrigger(ATTACK2);
                //Debug.Log("Attack 2");
                attackCount++;
            }
            else
            {
                animator.SetTrigger(ATTACK3);
                //Debug.Log("Attack3");
                attackCount = 0;
            }
        }
    }

    
    void ResetAttack()
    {
        readyToAttack = true;
        isEnemyHit = false;
        attacking = false;
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
        Debug.Log("Damage dealt: " +  damage);
    }
    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }

    public void ChangeAnimationState(string newState)
    {
        if(currentAnimationState == newState) return;

        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, transitionDuration);
    }
    public void Jab()
    {
        animator.SetTrigger("Jab");
        StartCoroutine(AttackCooldown());
    }

    public void Cross()
    {
        animator.SetTrigger("Cross");
        StartCoroutine(AttackCooldown());
    }
    public void Hook()
    {
        animator.SetTrigger("Hook");
    }

    public void UpperCut()
    {
        animator.SetTrigger("UpperCut");
    }

    public void Block()
    {
        animator.SetTrigger("Block");
        StartCoroutine(AttackCooldown());
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
