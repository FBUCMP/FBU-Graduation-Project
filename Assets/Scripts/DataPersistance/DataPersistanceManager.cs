using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Bir yerde data güncellenceðinde 2 tane yer aç, save ve load olarak
// save´de deðiþkeni güncelle loadda ise datayý çek örneðin coin sayýlarýn scriptine
// save´de data.coinCollected = this.coinCollected; loadda ise this.coinCollected = data.coinCollected; olacak.


public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "save.game";

    

    private GameData gameData; 

    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager instance { get; private set;} //veriyi public çeker ama özel setler

    private void Awake() //singleton yapýsý ile tek bir tane olmasý veri çakýþmasýný engelliyoruz. 
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
        LoadGame(); // Oyun baþladýðýnda yükle.
    }


    // ------------------ GAME DATA BÖLGESÝ ----------------
    public void NewGame()
    {
        this.gameData = new GameData(); //yeni obje oluþturuyoruz gayet basit
    }
    public void LoadGame()
    {
        // Data varsa yükle
        this.gameData = dataHandler.Load();
        // Data yoksa yeni data oluþtur.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // Yüklenen veriyi diðer her yere yolla ihtiyacý olan kullansýn.
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
        //Debug.Log("Olum sayisi yuklendi: " + gameData.deathCount);
    }
    public void SaveGame()
    {
        // Veriyi diðer scriptlere yolla ki güncellensinler.
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }
        //Debug.Log("Olum sayisi kaydedildi: " + gameData.deathCount);
        // Data handler kullanarak veriyi güncelle.
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>() 
            .OfType<IDataPersistance>(); //buradaki kod sayesinde bu scripti kullanan tamamýný Linq ile bulabiliyor ve listeleyebiliyoruz.

        return new List<IDataPersistance> (dataPersistanceObjects);

    }

}
