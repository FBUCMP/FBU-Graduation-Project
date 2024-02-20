using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Room
{

    public float[] map; // 1d array for json
    public int difficulty;
    public int index;
    public int width;
    public int height;

    public Room(float[] _map, int _width, int _height, int _dif = 0, int _index = 0)
    {
        map = _map;
        width = _width;
        height = _height;
        difficulty = _dif;
        index = _index;
    }
}
    
