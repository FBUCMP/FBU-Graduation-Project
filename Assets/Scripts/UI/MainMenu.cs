using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() //Play Button
    {
        SceneManager.LoadSceneAsync(1); // Play Butonuna bas�ld���nda file->build settings -> ka� nolu indeks sahnesi gelsin �rn: 1
        Time.timeScale = 1.0f; // Oyun ba�lay�p gameoverdan sonra tekrar ba�lat�ld���nda timescale bozulabiliyor.
    }

    public void QuitGame() // Quit Button
    {
        Application.Quit(); // Uygulamay� Kapat�r.
    }

}
