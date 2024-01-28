using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;


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


        Time.timeScale = 1.0f;
        gameOverUI.SetActive(false); // Olur da unutursak kapans�n 
        
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
