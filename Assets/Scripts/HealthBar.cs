using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // can bar�n�n azalmas� i�in gereken slider
    public Slider slider;

    // can bar�n�n say�sal de�erlere g�re renklerini ayarlamam�z� salayan de�i�ken
    public Gradient gradient;

    // can bar�n� dolduran arkadas
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


        // max can oldu�unda gradient 1fde ayarl� olan rengi ayarlar
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;

        // can�n de�i�mesi durumunda rengi ayarlar
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}