using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] CharacterStats characterStats;
    float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnHealthZero;

    private void Start()
    {
        currentHealth = characterStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = Mathf.Max(damage - characterStats.defense, 0);
        currentHealth -= totalDamage;

        OnHealthChanged?.Invoke(currentHealth/characterStats.maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthZero?.Invoke();
        }
    }
}
