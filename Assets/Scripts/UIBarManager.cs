using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{
    public Slider slider;
    public Animator animator;

    public void SetMaxSliderValue(float maxValue, float? value){
        slider.maxValue = maxValue;
        slider.value = value ?? maxValue;
    }

    public void SetSliderValue(float sliderValue){
        slider.value = sliderValue;
    }

    public void SetIntensePulse(bool isIntense){
        animator.SetBool("IntensePulsing", isIntense);
    }
    public void SetPulse(bool isPulsing){
        animator.SetBool("Pulsing", isPulsing);
    }
}
