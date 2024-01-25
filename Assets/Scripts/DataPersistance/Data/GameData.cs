using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;

    // Constructer oyun içerisinde kayýtlý data yoksa bunu kullanarak baþlayak.
    public GameData()
    {
        this.deathCount = 0;

    }
}
