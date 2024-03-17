using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// D��man kontrol�n� sa�layan MonoBehavior s�n�f�
public class EnemyController : MonoBehaviour
{
    private Enemy enemyData;  // D��man verilerini tutan nesne

    
    public void Initialize(Enemy enemy)
    {
        enemyData = enemy;
        // D��man�n �zelliklerini burada kullanabilirsiniz (�rne�in, health, damage, speed).
    }
    public void SetStats(float health, float speed, float power)
    {
        enemyData.Health = health;
        enemyData.Speed = speed;
        enemyData.Power = power;
    }

        // D��man�n di�er davran��lar�n� ekleyebilirsiniz.
    }

