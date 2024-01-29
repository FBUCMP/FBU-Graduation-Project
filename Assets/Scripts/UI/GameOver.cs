using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public void RestartGame() //Play Button
    {
        SceneManager.LoadSceneAsync(2);// Restart Butonuna bas�ld���nda file->build settings -> ka� nolu indeks sahnesi gelsin �rn: 1

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;

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
