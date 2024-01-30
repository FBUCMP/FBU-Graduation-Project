using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData data); // y�kledi�inde ise sadece okumas�na izin veriyoruz
    void SaveData(ref GameData data); // ref sebebi oyunu saveledi�inde scripti d�zenlemesi i�in izin veriyoruz.
}
