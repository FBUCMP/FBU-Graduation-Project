using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // can barýnýn azalmasý için gereken slider
    public Slider slider;

    // can barýnýn sayýsal deðerlere göre renklerini ayarlamamýzý salayan deðiþken
    public Gradient gradient;

    // can barýný dolduran arkadas
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;


        // max can olduðunda gradient 1fde ayarlý olan rengi ayarlar
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;

        // canýn deðiþmesi durumunda rengi ayarlar
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
