using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer; //Audo Mixer objesi.
    [SerializeField] private Slider musicSlider; //Slider objesi.
    [SerializeField] private Slider SFXSlider; //Slider objesi.


    private string musicVolume = "musicVolume"; // PlayerPrefs´te deðiþiklik yapma ihtimalimize karþýn deðiþkene atadým
    private string SFXVolume = "SFXVolume"; // PlayerPrefs´te deðiþiklik yapma ihtimalimize karþýn deðiþkene atadým

    private void Start()
    {
        if (PlayerPrefs.HasKey(musicVolume))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
        
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value; // Slider üzerindeki deðer ile volume deðerini eþitliyoruz float olma sebebi 0 ile 1 arasýnda bir deðer olmasý
        Mixer.SetFloat("Music", Mathf.Log10(volume)*20); // Mixerde deðerlerimiz negatif deðerlere kadar inmesine karþýn sliderda en fazla 0 olduðu için logaritmasýný alýyoruz ve 20 ile çarpýyoruz.
        PlayerPrefs.SetFloat(musicVolume, volume); // Ayarlarý kaydetme amacýyla PlayerPrefs´e yüklüyoruz.

    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value; // Slider üzerindeki deðer ile volume deðerini eþitliyoruz float olma sebebi 0 ile 1 arasýnda bir deðer olmasý
        Mixer.SetFloat("SFX", Mathf.Log10(volume) * 20); // Mixerde deðerlerimiz negatif deðerlere kadar inmesine karþýn sliderda en fazla 0 olduðu için logaritmasýný alýyoruz ve 20 ile çarpýyoruz.
        PlayerPrefs.SetFloat(SFXVolume, volume); // Ayarlarý kaydetme amacýyla PlayerPrefs´e yüklüyoruz.

    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(musicVolume); // PlayerPref içerisinde kullanýcýnýn ayarlamýþ olduðu düzeyi çekiyoruz.
        SFXSlider.value = PlayerPrefs.GetFloat(SFXVolume); // PlayerPref içerisinde kullanýcýnýn ayarlamýþ olduðu düzeyi çekiyoruz.

        SetMusicVolume(); 
        SetSFXVolume();
    }

}
