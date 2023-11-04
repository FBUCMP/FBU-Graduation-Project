using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // can barýnýn azalmasý için gereken slider
    public Slider slider;

    // can barýnýn sayýsal deðerlere göre renklerini ayarlamamýzý salayan deðiþken
    public Gradient gradient;

    // arka plan Image'i editörden güncellemek için
    public Image background;
    public Color backgroundColor;
    // can barýný dolduran arkadas
    public Image fill;

    // can barýnýn üzerine hpmizi yazan arkadas
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


        // max can olduðunda gradient 1fde ayarlý olan rengi ayarlar
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health, int maxHealth)
    {
        slider.value = (float)health / maxHealth * 100;
        Debug.Log("slider.value: " + slider.value);
        Debug.Log("slider.normalizedValue " + slider.normalizedValue);
        // canýn deðiþmesi durumunda rengi ayarlar
        fill.color = gradient.Evaluate(slider.normalizedValue);

        textCan.text = health + "/" + maxHealth;
    }

}
