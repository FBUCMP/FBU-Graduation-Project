using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    //[SerializeField] private GameObject PausePanel;
   

    public bool GetIsPaused()
    {
        return isPaused;
    }
    public void Continue()
    {
        if (isPaused)
        {
            ResumeGame();
            isPaused = false;
        } 
        else
        {
            Debug.LogError("Game not paused, user try to unpause");
        }
    }
    public void PauseGame()
    {
        GameManagerScript.Instance.pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        GameManagerScript.Instance.pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }
}
