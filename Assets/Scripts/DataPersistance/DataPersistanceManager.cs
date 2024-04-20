using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Bir yerde data g�ncellence�inde 2 tane yer a�, save ve load olarak
// save�de de�i�keni g�ncelle loadda ise datay� �ek �rne�in coin say�lar�n scriptine
// save�de data.coinCollected = this.coinCollected; loadda ise this.coinCollected = data.coinCollected; olacak.


public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "save.game";

    

    private GameData gameData; 

    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager instance { get; private set;} //veriyi public �eker ama �zel setler

    private void Awake() //singleton yap�s� ile tek bir tane olmas� veri �ak��mas�n� engelliyoruz. 
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one data persistance manager");
        }
        else
        {
            instance = this;
        }
        
    }

    private void Start()
    {

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame(); // Oyun ba�lad���nda y�kle.
    }


    // ------------------ GAME DATA B�LGES� ----------------
    public void NewGame()
    {
        this.gameData = new GameData(); //yeni obje olu�turuyoruz gayet basit
    }
    public void LoadGame()
    {
        // Data varsa y�kle
        this.gameData = dataHandler.Load();
        // Data yoksa yeni data olu�tur.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // Y�klenen veriyi di�er her yere yolla ihtiyac� olan kullans�n.
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
        //Debug.Log("Olum sayisi yuklendi: " + gameData.deathCount);
    }
    public void SaveGame()
    {
        // Veriyi di�er scriptlere yolla ki g�ncellensinler.
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }
        //Debug.Log("Olum sayisi kaydedildi: " + gameData.deathCount);
        // Data handler kullanarak veriyi g�ncelle.
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>() 
            .OfType<IDataPersistance>(); //buradaki kod sayesinde bu scripti kullanan tamam�n� Linq ile bulabiliyor ve listeleyebiliyoruz.

        return new List<IDataPersistance> (dataPersistanceObjects);

    }

}
