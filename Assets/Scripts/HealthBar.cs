using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HPSlider;

    public void SliderSet(float amount)
    {
        HPSlider.value = amount;
    }
    public void SliderSetMax(float amount)
    {
        HPSlider.maxValue = amount;
        SliderSet(amount);
    }
}
