using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GettingUpBar : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void UpdateGetUpBar(float value)
    {
        slider.value = value;
    }
    private void Update()
    {
        if (slider.value > 0)
        {
            slider.value -= 5 * Time.deltaTime;
        }
    }
    public float GetMaxValue()
    {
        return slider.maxValue;
    }

    public float GetCurrentValue()
    {
        return slider.value;
    }
}
