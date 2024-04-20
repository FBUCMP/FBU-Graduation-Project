using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame() //Play Button
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Restart Butonuna basýldýðýnda file->build settings -> kaç nolu indeks sahnesi gelsin örn: 1
        SceneManager.LoadScene(1);
        GameManagerScript.Instance.gameOverUI.SetActive(false);
        Time.timeScale = 1.0f;

    }
    public void MainMenu() //Play Button
    {
        GameManagerScript.Instance.gameOverUI.SetActive(false);
        SceneManager.LoadSceneAsync(0); // Restart Butonuna basýldýðýnda file->build settings -> kaç nolu indeks sahnesi gelsin örn: 1
    }
    public void ExitGame() // Quit Button
    {
        Application.Quit(); // Uygulamayý Kapatýr.
    }

}
