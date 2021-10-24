using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private Actor owner;
    private void Awake()
    {
        owner = GetComponentInParent<Actor>();
        slider = GetComponent<Slider>();
        slider.maxValue = owner.GetMaxHealth();
        slider.value = owner.GetHealth();
    }
    public void ChangeHealthValue(float valueChange)
    {
        slider.value += valueChange;
    }
}
