using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    public static EnemyUIManager Instance { get; set; }
    EnemyController enemy;

    [Header("Enemy's stamina")]
    public float minStamina;
    public float maxStamina;
    public float currentStamina;
    public StaminaBarEnemy staminaBar;
    public float originalStaminaValue = 0;

    [Header("Enemy's health")]
    public float minHealth;
    public float maxHealth;
    public float currentHealth;
    public HealthBarEnemy healthBar;
    public float originalHP = 0;

    [SerializeField] float smoothTransitionTime = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }
    private void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStamina <= minStamina && !enemy.isDefeated)
        {
            Debug.Log("KO!!!");
            enemy.KO();
        }
        if (currentHealth <= minHealth && !enemy.isDefeated)
        {
            Debug.Log("You Win!");
            enemy.KO();
        }
    }
    public IEnumerator SmoothHealthBarTransition(float newHealth)
    {
        originalHP = healthBar.GetHealth();
        float timeElapsed = 0;

        while (timeElapsed < smoothTransitionTime)
        {
            timeElapsed += Time.deltaTime;
            healthBar.UpdateHealthBar(Mathf.Lerp(originalHP, newHealth, timeElapsed / smoothTransitionTime));
            yield return null;


            healthBar.UpdateHealthBar(newHealth);
        }
    }
    public IEnumerator SmoothStaminaBarTransition(float newStaminaValue)
    {
        originalStaminaValue = staminaBar.GetStamina();
        float timeElapsed = 0;

        while (timeElapsed < smoothTransitionTime)
        {
            timeElapsed += Time.deltaTime;
            staminaBar.UpdateStaminaBar(Mathf.Lerp(originalStaminaValue, newStaminaValue, timeElapsed / smoothTransitionTime));
            yield return null;


            staminaBar.UpdateStaminaBar(newStaminaValue);
        }
    }
}
