using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector]public Vector2 SpawnRoomPos;
    [HideInInspector]public List<Collider2D> ValidColliders;
    private EnemyManager enemyManager;

    private List<EnemyData> EnemyDataList = new List<EnemyData>();
    private int NumberOfEnemiesToSpawn = 5;

    public void BeginProccess(List<EnemyData> EnemyDataList, int NumberOfEnemiesToSpawn, Vector2 SpawnRoomPos)
    {
        this.EnemyDataList = EnemyDataList;
        this.NumberOfEnemiesToSpawn = NumberOfEnemiesToSpawn;
        this.SpawnRoomPos = SpawnRoomPos;
        enemyManager = GetComponent<EnemyManager>();
        
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager component not found!");
        }
        else
        {
            Debug.Log("Enemy Manager is found: " + enemyManager.name);
            if (ValidColliders.Count <= 0)
            {
                Debug.LogWarning("No colliders found!");
                CreateColliders(NumberOfEnemiesToSpawn);
                SpawnCustomEnemies(NumberOfEnemiesToSpawn);
            }
            else
            {
                SpawnCustomEnemies(NumberOfEnemiesToSpawn);
            }
            DestroyChildColliders();
        }
    }

    void DestroyChildColliders()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Collider")
                Destroy(child.gameObject);
        }
    }

    void CreateColliders(int n)
    {
        if (SpawnRoomPos == null)
        {
            Debug.LogWarning("Spawn room position is not set!");
            return;
        }
        int s = 3;
        ValidColliders = new List<Collider2D>();
        int count = 0;
        while (count < n)
        {
            int X = UnityEngine.Random.Range(-40, 40);
            int Y = UnityEngine.Random.Range(-15, 15);
            Vector2 pos = new Vector2(X, Y) + SpawnRoomPos;
            Collider2D col = Physics2D.OverlapBox(pos, new Vector2(s, s), 0);
            if (col == null)
            {
                GameObject go = new GameObject("Collider");
                go.transform.position = pos;
                go.transform.parent = transform;
                BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(s, s);
                bc.transform.position = pos;

                ValidColliders.Add(bc);
                Debug.Log("Collider created at: " + pos);
                count++;
            }
            else
            {
                Debug.Log("Collider collided, couldn't spawn at: " + pos);
            }
        }

    }
    void SpawnCustomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            foreach (var enemyData in EnemyDataList)
            {
                SpawnEnemy(enemyData);
            }
        }
    }

    void SpawnEnemy(EnemyData enemyData)
    {
        Vector2 randomPosition = GetRandomColliderPosition();
        SpawnEnemyAtPosition(enemyData, randomPosition);
    }

    Vector2 GetRandomColliderPosition()
    {
        Collider2D randomCollider = ValidColliders[UnityEngine.Random.Range(0, ValidColliders.Count)];
        float minX = randomCollider.bounds.min.x;
        float maxX = randomCollider.bounds.max.x;
        float colliderTop = randomCollider.bounds.max.y;
        float offsetY = 1.0f;
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomY = colliderTop + offsetY;

        return new Vector2(randomX, randomY);
    }

    void SpawnEnemyAtPosition(EnemyData enemyData, Vector2 position)
    {
        GameObject enemyObject = Instantiate(enemyData.prefab, Vector3.zero, Quaternion.identity);
    }
}
