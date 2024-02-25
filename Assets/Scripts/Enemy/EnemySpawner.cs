using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string name;
    public float health;
    public float speed;
    public float power;
    public GameObject prefab;
    public float skillCooldown;
}

public class EnemySpawner : MonoBehaviour
{
    public Collider2D[] validColliders; // Collider'larýn listesi
    private EnemyManager enemyManager;

    public List<EnemyData> enemyDataList = new List<EnemyData>();
    public int numberOfEnemiesToSpawn = 5;

    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager component not found!");
        }
        else
        {
            SpawnCustomEnemies(numberOfEnemiesToSpawn);
            Debug.Log("Enemy Manager founded: " + enemyManager.name);
        }
    }

    void SpawnCustomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            foreach (var enemyData in enemyDataList)
            {
                SpawnEnemy(enemyData);
            }
        }
    }

    void SpawnEnemy(EnemyData enemyData)
    {
        // Rastgele bir konumu seç
        Vector2 randomPosition = GetRandomColliderPosition();

        // Düþmaný bu konumda oluþtur
        SpawnEnemyAtPosition(enemyData, randomPosition);
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

    void SpawnEnemyAtPosition(EnemyData enemyData, Vector2 position)
    {
        Debug.Log("SpawnEnemyAtPosition");
        if (enemyManager != null)
        {
            // Düþman oluþtur
            enemyManager.CreateEnemy(enemyData.name, enemyData.health, enemyData.speed, enemyData.power, position, enemyData.prefab);
        }
        else
        {
            Debug.LogError("Enemy Manager at SpawnEnemyAtPosition is NULL");
        }
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemySpawner spawner = (EnemySpawner)target;

        if (GUILayout.Button("Add Default Enemy"))
        {
            spawner.enemyDataList.Add(new EnemyData
            {
                name = "NewEnemy",
                health = 100f,
                speed = 5f,
                power = 20f,
                prefab = null, // Set your default prefab here
                skillCooldown = 5f
            });
        }
    }
}
#endif
