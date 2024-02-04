using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Düþman kontrolünü saðlayan MonoBehavior sýnýfý
public class EnemyController : MonoBehaviour
{
    private Enemy enemyData;  // Düþman verilerini tutan nesne

    // Düþmaný baþlat
    public void Initialize(Enemy enemy)
    {
        enemyData = enemy;
        // Düþmanýn özelliklerini burada kullanabilirsiniz (örneðin, health, damage, speed).
    }

    // Düþmanýn diðer davranýþlarýný ekleyebilirsiniz.
}

