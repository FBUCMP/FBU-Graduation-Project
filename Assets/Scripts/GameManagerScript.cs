using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    //public PauseMenu pauseMenu;


    /// <summary>
    /// Singleton deseni oluþturuyoruz yani oyun içerisinde sadece bir adet oluþturulmasý gereken ve oyun boyunca 
    /// nesneleri kontrol edip, statik kalmasý gereken deðiþkenleri yönettiðimiz kod bloðu olacak.
    /// blok buradan itibaren baþlamakta.
    /// </summary>

    private static GameManagerScript _instance; // Gamemanager´ýn bir adet kopyasýný/örneðini içeren kod satýrý.

    public static GameManagerScript Instance //Instance adýnda bir özellik oluþturduk ve buradan get ile eriþim saðladýk.
    {
        get
        {
            if (_instance == null) // En baþta burasý var mý yok mu diye kontrol ediyoruz ve yoksa buradan bir kopya oluþturuyoruz.
            {
                _instance = FindObjectOfType<GameManagerScript>(); //Sahne içerisinde herhangi bir biçimde gamemanagerscript var mý diye kontrol ediyoruz. 
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = null;
            DontDestroyOnLoad(this.gameObject); // Sahne deðiþtiðinde yok olmasýný engelliyoruz
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// BÝTÝÞ
    private void Start()
    {
        pauseMenuUI = pauseMenuUI.transform.Find("InGameUIManager/PauseScreen/PauseMenuScreen").gameObject;
        /*pauseMenu = FindObjectOfType<PauseMenu>();
        if(pauseMenu == null)
        {
            Debug.LogError("PauseMenu not found!");
        }
        */
        Time.timeScale = 1.0f;
        gameOverUI.SetActive(false); 
        pauseMenuUI.SetActive(false); 
       
    }

    private void Update()
    {
        //Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuUI != null)
        {
            if (pauseMenuUI.activeSelf)
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Return) && pauseMenuUI == null)
        {
            Debug.LogError("Pause Menu is null");
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
