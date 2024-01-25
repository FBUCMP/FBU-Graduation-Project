using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Bir yerde data güncellenceðinde 2 tane yer aç, save ve load olarak
// save´de deðiþkeni güncelle loadda ise datayý çek örneðin coin sayýlarýn scriptine
// save´de data.coinCollected = this.coinCollected; loadda ise this.coinCollected = data.coinCollected; olacak.


public class DataPersistanceManager : MonoBehaviour
{
    private GameData gameData; 
    public static DataPersistanceManager instance { get; private set;} //veriyi public çeker ama özel setler

    private void Awake() //singleton yapýsý ile tek bir tane olmasý veri çakýþmasýný engelliyoruz. 
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one data persistance manager");
        }
        instance = this;
    }

    private void Start()
    {
        LoadGame(); // Oyun baþladýðýnda yükle.
    }


    // ------------------ GAME DATA BÖLGESÝ ----------------
    public void NewGame()
    {
        this.gameData = new GameData(); //yeni obje oluþturuyoruz gayet basit
    }
    public void LoadGame()
    {
        // TODO - Data varsa yükle
  
        // Data yoksa yeni data oluþtur.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // TODO - Yüklenen veriyi diðer her yere yolla ihtiyacý olan kullansýn.
    }
    public void SaveGame()
    {
        // TODO - Veriyi diðer scriptlere yolla ki güncellensinler.

        // TODO - Data handler kullanarak veriyi güncelle.
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
