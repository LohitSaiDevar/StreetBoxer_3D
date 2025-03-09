using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; set; }
    PlayerController player;

    [Header("Player's stamina")]
    public float minStamina;
    public float maxStamina;
    public float currentStamina;
    public StaminaBar staminaBar;
    public float originalStaminaValue = 0;

    [Header("Player's health")]
    public float minHealth;
    public float maxHealth;
    public float currentHealth;
    public HealthBarPlayer healthBarPlayer;
    public float originalHP = 0;

    [SerializeField] float smoothTransitionTime = 0.5f;

    public GettingUpBar getUpBar;
    public float gettingUpMeterValue = 10;
    public float originalGetUpValue = 0;


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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStamina <= minStamina && !player.isDefeated)
        {
            Debug.Log("KO!!!");
            player.isDefeated = true;
            player.KO();
            getUpBar.gameObject.SetActive(true);
        }
        if (currentHealth <= minHealth && !player.isDefeated)
        {
            Debug.Log("You Win!");
            player.KO();
        }
    }
    public IEnumerator SmoothHealthBarTransition(float newHealth)
    {
        originalHP = healthBarPlayer.GetHealth();
        float timeElapsed = 0;

        while (timeElapsed < smoothTransitionTime)
        {
            timeElapsed += Time.deltaTime;
            healthBarPlayer.UpdateHealthBar(Mathf.Lerp(originalHP, newHealth, timeElapsed/smoothTransitionTime));
            yield return null;

            
            healthBarPlayer.UpdateHealthBar(newHealth);
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

    public IEnumerator SmoothGetUpBarTransition(float newValue)
    {
        originalGetUpValue = getUpBar.GetCurrentValue();
        float timeElapsed = 0;

        while (timeElapsed < smoothTransitionTime)
        {
            timeElapsed += Time.deltaTime;
            getUpBar.UpdateGetUpBar(Mathf.Lerp(originalGetUpValue, newValue, timeElapsed / smoothTransitionTime));
            yield return null;


            getUpBar.UpdateGetUpBar(newValue);
        }
    }
}
