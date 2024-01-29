using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() //Play Button
    {
        SceneManager.LoadSceneAsync(1); // Play Butonuna basýldýðýnda file->build settings -> kaç nolu indeks sahnesi gelsin örn: 1
    }

    public void QuitGame() // Quit Button
    {
        Application.Quit(); // Uygulamayý Kapatýr.
    }

}
