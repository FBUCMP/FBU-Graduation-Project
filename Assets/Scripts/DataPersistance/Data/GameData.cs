using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;

    // Constructer oyun i�erisinde kay�tl� data yoksa bunu kullanarak ba�layak.
    public GameData()
    {
        this.deathCount = 0;

    }
}
