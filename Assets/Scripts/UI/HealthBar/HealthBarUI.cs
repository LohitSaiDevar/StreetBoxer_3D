using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    public Health health;
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        health.OnHealthChanged += UpdateHealthBar;
    }

    public void UpdateHealthBar(float healthValue)
    {
        slider.value = healthValue;
    }

}
