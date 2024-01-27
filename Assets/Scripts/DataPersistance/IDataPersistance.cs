using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData data); // yüklediðinde ise sadece okumasýna izin veriyoruz
    void SaveData(ref GameData data); // ref sebebi oyunu savelediðinde scripti düzenlemesi için izin veriyoruz.
}
