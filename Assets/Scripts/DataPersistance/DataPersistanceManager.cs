using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Bir yerde data g�ncellence�inde 2 tane yer a�, save ve load olarak
// save�de de�i�keni g�ncelle loadda ise datay� �ek �rne�in coin say�lar�n scriptine
// save�de data.coinCollected = this.coinCollected; loadda ise this.coinCollected = data.coinCollected; olacak.


public class DataPersistanceManager : MonoBehaviour
{
    private GameData gameData; 
    public static DataPersistanceManager instance { get; private set;} //veriyi public �eker ama �zel setler

    private void Awake() //singleton yap�s� ile tek bir tane olmas� veri �ak��mas�n� engelliyoruz. 
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one data persistance manager");
        }
        instance = this;
    }

    private void Start()
    {
        LoadGame(); // Oyun ba�lad���nda y�kle.
    }


    // ------------------ GAME DATA B�LGES� ----------------
    public void NewGame()
    {
        this.gameData = new GameData(); //yeni obje olu�turuyoruz gayet basit
    }
    public void LoadGame()
    {
        // TODO - Data varsa y�kle
  
        // Data yoksa yeni data olu�tur.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // TODO - Y�klenen veriyi di�er her yere yolla ihtiyac� olan kullans�n.
    }
    public void SaveGame()
    {
        // TODO - Veriyi di�er scriptlere yolla ki g�ncellensinler.

        // TODO - Data handler kullanarak veriyi g�ncelle.
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
