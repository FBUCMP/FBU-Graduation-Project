using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData data); // yüklediğinde ise sadece okumasına izin veriyoruz
    void SaveData(ref GameData data); // ref sebebi oyunu savelediğinde scripti düzenlemesi için izin veriyoruz.
}
