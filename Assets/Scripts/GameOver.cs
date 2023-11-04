using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame() //Play Button
    {
        SceneManager.LoadSceneAsync(1); // Restart Butonuna basýldýðýnda file->build settings -> kaç nolu indeks sahnesi gelsin örn: 1
    }
    public void MainMenu() //Play Button
    {
        SceneManager.LoadSceneAsync(0); // Restart Butonuna basýldýðýnda file->build settings -> kaç nolu indeks sahnesi gelsin örn: 1
    }
    public void ExitGame() // Quit Button
    {
        Application.Quit(); // Uygulamayý Kapatýr.
    }

}
