using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadGame(GameData data); // y�kledi�inde ise sadece okumas�na izin veriyoruz
    void SaveGame(ref GameData data); // ref sebebi oyunu saveledi�inde scripti d�zenlemesi i�in izin veriyoruz.
}
