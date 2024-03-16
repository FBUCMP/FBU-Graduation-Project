using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Düþman kontrolünü saðlayan MonoBehavior sýnýfý
public class EnemyController : MonoBehaviour
{
    private Enemy enemyData;  // Düþman verilerini tutan nesne

    
    public void Initialize(Enemy enemy)
    {
        enemyData = enemy;
        // Düþmanýn özelliklerini burada kullanabilirsiniz (örneðin, health, damage, speed).
    }
    public void SetStats(float health, float speed, float power)
    {
        enemyData.Health = health;
        enemyData.Speed = speed;
        enemyData.Power = power;
    }

        // Düþmanýn diðer davranýþlarýný ekleyebilirsiniz.
    }

