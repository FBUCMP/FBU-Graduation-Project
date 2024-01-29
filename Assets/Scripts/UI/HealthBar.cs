using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
//ISSUE
//Oyuncunun ne kadar canýnýn kaldýðý oyun ekranýnýn bir köþesinde bir bar þeklinde görünecek:

//Barýn doluluðu, oyuncunun canýnýn miktarý ile deðil, canýnýn yüzde kaçýnýn dolu olduðuna göre deðiþecek. Örneðin oyuncunun caný 100/1000 ise barýn %10'u dolu olacak.
//Barýn üzerinde ayrýca "can/max can" yazýlý olarak da gösterilecek. Örn "50/75" gibi.
//Barýn rengi oyuncunun canýnýn doluluðuna göre deðiþecek. Örneðin oyuncunun caný tam ise bar yeþil, caný bittiyse bar kýrmýzý; ara can deðerleri için de deðere göre yeþilden
//kýrmýzýya doðru deðiþecek. Örneðin oyuncunun caný %81 ise yeþile daha yakýn, %15 ise kýrmýzýya daha yakýn gibi.
//Bar rengi için Unity'deki Gradient component'ýndan faydalanabilirsiniz, ama ara sýnavda Gradient kullanmadan bunu yapmak isteseniz nasýl yapacaðýnýzý anlatmanýz gerekebilir.

	Can barý eklendi, üzerine hp/maxHp þeklinde yazý eklendi. Can ve max Can yüzdesine göre ayarlanan bir gradient eklendi.

	Gradient %lik olarak:
	80-100 arasý yeþil
	60-80 arasý sarý
	30-60 arasý turuncu/sarý
	15-30 arasý kýrmýzý/sarý
	0-15 arasý kýrmýzý renklere ayarlandý.


*/
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

        // gerekli componentler yoksa diye kontrol

        // can barýmýzda value deðiþtikçe boyu deðiþen arkadaþ
        if (slider == null)
        {
            Debug.Log("slider null");
            this.AddComponent<Slider>();
            slider = GetComponent<Slider>();
        }

        // fill.color içerisinde renk ayarý yaptýðýmýz arkadaþ
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

    // max health ayarý
    public void SetMaxHealth(int health, int maxHealth)
    {
        slider.maxValue = (float)health / maxHealth * 100;
        slider.value = (float)health / maxHealth * 100;


        // max can olduðunda gradient 1fde ayarlý olan rengi ayarlar
        fill.color = gradient.Evaluate(1f);
    }


    public void SetHealth(int health, int maxHealth)
    {

        // canýn %liðine göre slider kayýyor
        slider.value = (float)health / maxHealth * 100;
        //Debug.Log("slider.value: " + slider.value);
        //Debug.Log("slider.normalizedValue " + slider.normalizedValue);

        // canýn deðiþmesi durumunda rengi ayarlar
        fill.color = gradient.Evaluate(slider.normalizedValue);

        textCan.text = health + "/" + maxHealth;
    }

}