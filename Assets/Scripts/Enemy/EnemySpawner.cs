using System;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public Collider2D[] validColliders; // Collider'larýn listesi
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
            // Rastgele bir konumu seç
            Vector2 randomPosition = GetRandomColliderPosition();

            // Düþmaný bu konumda oluþtur
            SpawnEnemyAtPosition(randomPosition);
        }
    }

    Vector2 GetRandomColliderPosition()
    {
        // Rastgele bir collider seç
        Collider2D randomCollider = validColliders[UnityEngine.Random.Range(0, validColliders.Length)];

        // Collider sýnýrlarý içinde rastgele bir konum oluþtur
        float minX = randomCollider.bounds.min.x;
        float maxX = randomCollider.bounds.max.x;

        // Yüksekliði bul
        float colliderTop = randomCollider.bounds.max.y;

        // Offset deðeri, collider'ýn üstüne yerleþtirmek için kullanýlýr
        float offsetY = 1.0f; // Ýstenilen bir offset deðeri

        // Collider'ýn üst kýsmýna bir offset ekleyerek rastgele bir konum oluþtur
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = colliderTop + offsetY;

        return new Vector2(randomX, randomY);
    }

    void SpawnEnemyAtPosition(Vector2 position)
    {
        Debug.Log("SpawnEnemyAtPosition");
        if (enemyManager != null)
        {
            // Düþman oluþtur
            Debug.Log("Düþman oluþturma deneniyor");
            enemyManager.CreateEnemy(100f, 5f, 20f, position);
        }
        else
        {
            Debug.LogError("Enemy Manager at SpawnEnemyAtPosition is NULL");
        }
    }
}
