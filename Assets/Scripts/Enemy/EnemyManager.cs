using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// D��man y�netimini sa�layan ScriptableObject s�n�f�
[CreateAssetMenu(fileName = "EnemyManager", menuName = "ScriptableObjects/EnemyManager", order = 1)]
public class EnemyManager : ScriptableObject
{
    public Enemy[] enemies;  // D��manlar�n listesi

    // Belirtilen d��man� spawn et
    public void SpawnEnemy(int index, Vector3 spawnPosition)
    {
        if (index >= 0 && index < enemies.Length)
        {
            // D��man� instantiate et
            GameObject enemyObject = new GameObject(enemies[index].enemyName);
            EnemyController enemyController = enemyObject.AddComponent<EnemyController>();
            enemyController.Initialize(enemies[index]);

            // D��man� belirtilen pozisyona yerle�tir
            enemyObject.transform.position = spawnPosition;
        }
        else
        {
            Debug.LogError("Invalid enemy index.");
        }
    }
}