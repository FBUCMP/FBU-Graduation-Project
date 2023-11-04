using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame() //Play Button
    {
        SceneManager.LoadSceneAsync(1); // Restart Butonuna bas�ld���nda file->build settings -> ka� nolu indeks sahnesi gelsin �rn: 1
    }
    public void MainMenu() //Play Button
    {
        SceneManager.LoadSceneAsync(0); // Restart Butonuna bas�ld���nda file->build settings -> ka� nolu indeks sahnesi gelsin �rn: 1
    }
    public void ExitGame() // Quit Button
    {
        Application.Quit(); // Uygulamay� Kapat�r.
    }

}
