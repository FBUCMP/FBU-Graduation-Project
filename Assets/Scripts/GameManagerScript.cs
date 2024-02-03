using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    //public PauseMenu pauseMenu;


    /// <summary>
    /// Singleton deseni olu�turuyoruz yani oyun i�erisinde sadece bir adet olu�turulmas� gereken ve oyun boyunca 
    /// nesneleri kontrol edip, statik kalmas� gereken de�i�kenleri y�netti�imiz kod blo�u olacak.
    /// blok buradan itibaren ba�lamakta.
    /// </summary>

    private static GameManagerScript _instance; // Gamemanager��n bir adet kopyas�n�/�rne�ini i�eren kod sat�r�.

    public static GameManagerScript Instance //Instance ad�nda bir �zellik olu�turduk ve buradan get ile eri�im sa�lad�k.
    {
        get
        {
            if (_instance == null) // En ba�ta buras� var m� yok mu diye kontrol ediyoruz ve yoksa buradan bir kopya olu�turuyoruz.
            {
                _instance = FindObjectOfType<GameManagerScript>(); //Sahne i�erisinde herhangi bir bi�imde gamemanagerscript var m� diye kontrol ediyoruz. 
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = null;
            DontDestroyOnLoad(this.gameObject); // Sahne de�i�ti�inde yok olmas�n� engelliyoruz
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    /// B�T��
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
