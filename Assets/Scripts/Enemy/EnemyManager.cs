using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Düþman yönetimini saðlayan ScriptableObject sýnýfý
[CreateAssetMenu(fileName = "EnemyManager", menuName = "ScriptableObjects/EnemyManager", order = 1)]
public class EnemyManager : ScriptableObject
{
    public Enemy[] enemies;  // Düþmanlarýn listesi

    // Belirtilen düþmaný spawn et
    public void SpawnEnemy(int index, Vector3 spawnPosition)
    {
        if (index >= 0 && index < enemies.Length)
        {
            // Düþmaný instantiate et
            GameObject enemyObject = new GameObject(enemies[index].enemyName);
            EnemyController enemyController = enemyObject.AddComponent<EnemyController>();
            enemyController.Initialize(enemies[index]);

            // Düþmaný belirtilen pozisyona yerleþtir
            enemyObject.transform.position = spawnPosition;
        }
        else
        {
            Debug.LogError("Invalid enemy index.");
        }
    }
}