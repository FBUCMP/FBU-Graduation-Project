using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void Continue()
    {

        ResumeGame();
    }
    public void PauseGame()
    {
        GameManagerScript.Instance.pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        GameManagerScript.Instance.pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
    }
    public void QuitUI() 
    {
        SceneManager.LoadSceneAsync(0); 
    }
}
