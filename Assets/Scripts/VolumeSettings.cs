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


    private string musicVolume = "musicVolume"; // PlayerPrefs�te de�i�iklik yapma ihtimalimize kar��n de�i�kene atad�m
    private string SFXVolume = "SFXVolume"; // PlayerPrefs�te de�i�iklik yapma ihtimalimize kar��n de�i�kene atad�m

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
        float volume = musicSlider.value; // Slider �zerindeki de�er ile volume de�erini e�itliyoruz float olma sebebi 0 ile 1 aras�nda bir de�er olmas�
        Mixer.SetFloat("Music", Mathf.Log10(volume)*20); // Mixerde de�erlerimiz negatif de�erlere kadar inmesine kar��n sliderda en fazla 0 oldu�u i�in logaritmas�n� al�yoruz ve 20 ile �arp�yoruz.
        PlayerPrefs.SetFloat(musicVolume, volume); // Ayarlar� kaydetme amac�yla PlayerPrefs�e y�kl�yoruz.

    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value; // Slider �zerindeki de�er ile volume de�erini e�itliyoruz float olma sebebi 0 ile 1 aras�nda bir de�er olmas�
        Mixer.SetFloat("SFX", Mathf.Log10(volume) * 20); // Mixerde de�erlerimiz negatif de�erlere kadar inmesine kar��n sliderda en fazla 0 oldu�u i�in logaritmas�n� al�yoruz ve 20 ile �arp�yoruz.
        PlayerPrefs.SetFloat(SFXVolume, volume); // Ayarlar� kaydetme amac�yla PlayerPrefs�e y�kl�yoruz.

    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(musicVolume); // PlayerPref i�erisinde kullan�c�n�n ayarlam�� oldu�u d�zeyi �ekiyoruz.
        SFXSlider.value = PlayerPrefs.GetFloat(SFXVolume); // PlayerPref i�erisinde kullan�c�n�n ayarlam�� oldu�u d�zeyi �ekiyoruz.

        SetMusicVolume(); 
        SetSFXVolume();
    }

}
