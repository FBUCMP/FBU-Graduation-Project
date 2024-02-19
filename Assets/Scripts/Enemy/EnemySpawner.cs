using System;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public Collider2D[] validColliders; // Collider'lar�n listesi
    private EnemyManager enemyManager;

    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager component not found!");
        }
        else
        {
            SpawnRandomEnemies(5);
            Debug.Log("Enemy Manager founded: " + enemyManager.name);
        }
    }

    void SpawnRandomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Rastgele bir konumu se�
            Vector2 randomPosition = GetRandomColliderPosition();

            // D��man� bu konumda olu�tur
            SpawnEnemyAtPosition(randomPosition);
        }
    }

    Vector2 GetRandomColliderPosition()
    {
        // Rastgele bir collider se�
        Collider2D randomCollider = validColliders[UnityEngine.Random.Range(0, validColliders.Length)];

        // Collider s�n�rlar� i�inde rastgele bir konum olu�tur
        float minX = randomCollider.bounds.min.x;
        float maxX = randomCollider.bounds.max.x;

        // Y�ksekli�i bul
        float colliderTop = randomCollider.bounds.max.y;

        // Offset de�eri, collider'�n �st�ne yerle�tirmek i�in kullan�l�r
        float offsetY = 1.0f; // �stenilen bir offset de�eri

        // Collider'�n �st k�sm�na bir offset ekleyerek rastgele bir konum olu�tur
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = colliderTop + offsetY;

        return new Vector2(randomX, randomY);
    }

    void SpawnEnemyAtPosition(Vector2 position)
    {
        Debug.Log("SpawnEnemyAtPosition");
        if (enemyManager != null)
        {
            // D��man olu�tur
            Debug.Log("D��man olu�turma deneniyor");
            enemyManager.CreateEnemy(100f, 5f, 20f, position);
        }
        else
        {
            Debug.LogError("Enemy Manager at SpawnEnemyAtPosition is NULL");
        }
    }
}
