using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
//ISSUE
//Oyuncunun ne kadar can�n�n kald��� oyun ekran�n�n bir k��esinde bir bar �eklinde g�r�necek:

//Bar�n dolulu�u, oyuncunun can�n�n miktar� ile de�il, can�n�n y�zde ka��n�n dolu oldu�una g�re de�i�ecek. �rne�in oyuncunun can� 100/1000 ise bar�n %10'u dolu olacak.
//Bar�n �zerinde ayr�ca "can/max can" yaz�l� olarak da g�sterilecek. �rn "50/75" gibi.
//Bar�n rengi oyuncunun can�n�n dolulu�una g�re de�i�ecek. �rne�in oyuncunun can� tam ise bar ye�il, can� bittiyse bar k�rm�z�; ara can de�erleri i�in de de�ere g�re ye�ilden
//k�rm�z�ya do�ru de�i�ecek. �rne�in oyuncunun can� %81 ise ye�ile daha yak�n, %15 ise k�rm�z�ya daha yak�n gibi.
//Bar rengi i�in Unity'deki Gradient component'�ndan faydalanabilirsiniz, ama ara s�navda Gradient kullanmadan bunu yapmak isteseniz nas�l yapaca��n�z� anlatman�z gerekebilir.

	Can bar� eklendi, �zerine hp/maxHp �eklinde yaz� eklendi. Can ve max Can y�zdesine g�re ayarlanan bir gradient eklendi.

	Gradient %lik olarak:
	80-100 aras� ye�il
	60-80 aras� sar�
	30-60 aras� turuncu/sar�
	15-30 aras� k�rm�z�/sar�
	0-15 aras� k�rm�z� renklere ayarland�.


*/
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

        // gerekli componentler yoksa diye kontrol

        // can bar�m�zda value de�i�tik�e boyu de�i�en arkada�
        if (slider == null)
        {
            Debug.Log("slider null");
            this.AddComponent<Slider>();
            slider = GetComponent<Slider>();
        }

        // fill.color i�erisinde renk ayar� yapt���m�z arkada�
        if (gradient == null)
        {
            Debug.Log("grad null");
            gradient = new Gradient();
        }

        // fill 
        if (fill == null)
        {
            Debug.Log("fill null");
            this.AddComponent<Image>();
            fill = GetComponent<Image>();
        }

        if (textCan == null)
        {
            Debug.Log("text can null");
            this.AddComponent<Text>();
            textCan = GetComponent<Text>();
        }
    }

    // max health ayar�
    public void SetMaxHealth(int health, int maxHealth)
    {
        slider.maxValue = (float)health / maxHealth * 100;
        slider.value = (float)health / maxHealth * 100;


        // max can oldu�unda gradient 1fde ayarl� olan rengi ayarlar
        fill.color = gradient.Evaluate(1f);
    }


    public void SetHealth(int health, int maxHealth)
    {

        // can�n %li�ine g�re slider kay�yor
        slider.value = (float)health / maxHealth * 100;
        //Debug.Log("slider.value: " + slider.value);
        //Debug.Log("slider.normalizedValue " + slider.normalizedValue);

        // can�n de�i�mesi durumunda rengi ayarlar
        fill.color = gradient.Evaluate(slider.normalizedValue);

        textCan.text = health + "/" + maxHealth;
    }

}