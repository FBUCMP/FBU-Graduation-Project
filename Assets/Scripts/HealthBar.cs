using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // can bar�n�n azalmas� i�in gereken slider
    public Slider slider;

    // can bar�n�n say�sal de�erlere g�re renklerini ayarlamam�z� salayan de�i�ken
    public Gradient gradient;

    // arka plan Image'i edit�rden g�ncellemek i�in
    public Image background;
    public Color backgroundColor;
    // can bar�n� dolduran arkadas
    public Image fill;

    // can bar�n�n �zerine hpmizi yazan arkadas
    public Text textCan;

    private void Start()
    {
        backgroundColor = new Color(0, 255, 255); // cyan
        background.color = backgroundColor;

        if (slider == null)
        {
            Debug.Log("slider null");
            this.AddComponent<Slider>();
            slider = GetComponent<Slider>();
        }

        if (gradient == null)
        {
            Debug.Log("grad null");
            gradient = new Gradient();
        }
        
        if(fill == null)
        {
            Debug.Log("fill null");
            this.AddComponent<Image>();
            fill = GetComponent<Image>();
        }
        
        if(textCan == null)
        {
            Debug.Log("text can null");
            this.AddComponent<Text>();
            textCan = GetComponent<Text>();
        }
    }
    public void SetMaxHealth(int health, int maxHealth)
    {
        slider.maxValue = (float)health / maxHealth * 100;
        slider.value = (float)health / maxHealth * 100;


        // max can oldu�unda gradient 1fde ayarl� olan rengi ayarlar
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health, int maxHealth)
    {
        slider.value = (float)health / maxHealth * 100;
        Debug.Log("slider.value: " + slider.value);
        Debug.Log("slider.normalizedValue " + slider.normalizedValue);
        // can�n de�i�mesi durumunda rengi ayarlar
        fill.color = gradient.Evaluate(slider.normalizedValue);

        textCan.text = health + "/" + maxHealth;
    }

}
