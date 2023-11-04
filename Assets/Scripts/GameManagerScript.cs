using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    private void Start()
    {
        Time.timeScale = 1.0f;
        gameOverUI.SetActive(false); // Olur da unutursak kapansýn 
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
       
    }

    private void Update()
    {
        
        if(gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
