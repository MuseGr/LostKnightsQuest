using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxStamina(int st)
    {
        slider.maxValue = st;
        slider.value = st;
    }
    public void SetStamina(int st)
    {
        slider.value = st;
    }
}
