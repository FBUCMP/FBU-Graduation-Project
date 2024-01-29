using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //Path.Combine ile farkl� OS�lar i�in directory buluyoruz
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try 
            {
                //Load the serialized data
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Deserialized data to C#
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            } 
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }

        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //Path.Combine ile farkl� OS�lar i�in directory buluyoruz
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //Yoksa kay�t dosyas�n� olu�turucaz.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Game Datay� C#�tan Json yap�caz
            string dataToStore = JsonUtility.ToJson(data,true);

            //Datay� yaz�yoruz
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream)) 
                {
                writer.Write(dataToStore);
                }
            }

        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e );
        }
    }

}
