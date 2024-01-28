using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;


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
