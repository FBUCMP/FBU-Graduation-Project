using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadGame(GameData data); // yüklediðinde ise sadece okumasýna izin veriyoruz
    void SaveGame(ref GameData data); // ref sebebi oyunu savelediðinde scripti düzenlemesi için izin veriyoruz.
}
