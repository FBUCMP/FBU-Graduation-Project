using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;

    public Image fill;

    private void Start()
    {
        if (slider == null)
        {
            this.AddComponent<Slider>();
            slider = GetComponent<Slider>();
        }
        if (gradient == null)
        {
            gradient = new Gradient();
        }
        
        if(fill == null)
        {
            this.AddComponent<Image>();
            fill = GetComponent<Image>();
        }
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
