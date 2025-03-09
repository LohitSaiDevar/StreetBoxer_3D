using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void UpdateHealthBar(float currentHealth)
    {
        slider.value = currentHealth;
    }
    public float GetHealth()
    {
        return slider.value;
    }
}
