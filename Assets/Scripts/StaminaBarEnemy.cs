using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarEnemy : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void UpdateStaminaBar(float currentStamina)
    {
        slider.value = currentStamina;
    }
    public float GetStamina()
    {
        return slider.value;
    }
}
