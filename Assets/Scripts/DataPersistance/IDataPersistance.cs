using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadGame(GameData data); // yüklediğinde ise sadece okumasına izin veriyoruz
    void SaveGame(ref GameData data); // ref sebebi oyunu savelediğinde scripti düzenlemesi için izin veriyoruz.
}
