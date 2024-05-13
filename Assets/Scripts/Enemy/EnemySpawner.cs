using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector]public GameObject SpawnRoom;
    [HideInInspector]public List<Collider2D> ValidColliders;
    [HideInInspector]public List<EnemyHealth> enemyHealthList;
    private EnemyManager enemyManager;

    private List<EnemyData> EnemyDataList = new List<EnemyData>();
    private GameObject enemyHolder;
    private int NumberOfEnemiesToSpawn = 5;
    
    public void BeginProccess(List<EnemyData> EnemyDataList, int NumberOfEnemiesToSpawn, GameObject SpawnRoom, GameObject enemyHolder)
    {
        //Debug.Log("BeginProcess, SpawnRoomPos: "+ SpawnRoomPos);


        enemyManager = FindObjectOfType<EnemyManager>();

        enemyHealthList = new List<EnemyHealth>(); // create a new list

        this.EnemyDataList = EnemyDataList;
        this.NumberOfEnemiesToSpawn = NumberOfEnemiesToSpawn;
        this.SpawnRoom = SpawnRoom;
        this.enemyHolder = enemyHolder;
        if(this.EnemyDataList.Count > 0)
        {
            Debug.Log("EnemyDataList: " + this.EnemyDataList);
        } else
        {
            Debug.Log("EnemyDataList is Empty");
        }


        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager component not found!");
        }
        else
        {
            Debug.Log("Enemy Manager is found: " + enemyManager.name);
            
            DestroyChildColliders();
            CreateColliders(NumberOfEnemiesToSpawn * EnemyDataList.Count);
           
            SpawnCustomEnemies(NumberOfEnemiesToSpawn);
            
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

    void CreateColliders(int n) // creates n colliders to check if the area is empty and uses heatmaps to check if the area is empty
    {
        if (SpawnRoom == null)
        {
            Debug.LogWarning("Spawn room is not set!");
            return;
        }
        int s = 3;
        ValidColliders = new List<Collider2D>();
        int count = 0;
        int attempts = 0;
        int maxAttempts = 10000;
        while (count < n)
        {
            if (attempts > maxAttempts)
            {
                Debug.LogWarning("Max attempts reached, could not create enough colliders!");
                break;
            }
            int X = UnityEngine.Random.Range(-40, 40); // random position in the room bounds hand written
            int Y = UnityEngine.Random.Range(-20, 20);
            HeatMap heatMap = SpawnRoom.GetComponent<HeatMap>(); // heatmaps are used to check if the area is empty
            
            Vector2 pos = new Vector3(X, Y) + SpawnRoom.transform.position;
            Collider2D col = Physics2D.OverlapBox(pos, new Vector2(s, s), 0);
            if (  col == null && heatMap.IsInEmpty( new Bounds(pos, new Vector3(s,s)), 0.5f )  )
            {
                GameObject go = new GameObject("Collider");
                go.transform.position = pos;
                go.transform.parent = transform;
                BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(s, s);
                bc.transform.position = pos;

                ValidColliders.Add(bc);
                heatMap.UpdateHeatMap(bc.bounds); // update heatmap turn the area enemy occupied to 1
                //Debug.Log("Collider created at: " + pos);
                count++;
            }
            else
            {
                //Debug.Log("Collider collided, couldn't spawn at: " + pos);
            }
            attempts++;
        }
        Debug.Log("Created " + count + " colliders");
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
        if (ValidColliders.Count == 0)
        {
            Debug.LogWarning("No valid colliders to spawn enemy!");
            
        }
        else
        {
            //Vector2 randomPosition = GetRandomColliderPosition();
            Vector2 spawnpos = ValidColliders[0].transform.position;
            ValidColliders.RemoveAt(0);
            SpawnEnemyAtPosition(enemyData, spawnpos);
        }
    }

    Vector2 GetRandomColliderPosition()
    {
        Collider2D randomCollider = ValidColliders[UnityEngine.Random.Range(0, ValidColliders.Count)];
        return randomCollider.transform.position; // valid colliders dont collide with walls, so we can use their position directly
    }

    void SpawnEnemyAtPosition(EnemyData enemyData, Vector2 position)
    {
        GameObject enemyObject = Instantiate(enemyData.prefab, position, Quaternion.identity);
        enemyObject.transform.parent = enemyHolder.transform;
        if (enemyObject.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.maxHealth = (int)enemyData.health;
            enemyHealth.currentHealth = enemyHealth.maxHealth;
            enemyHealthList.Add(enemyHealth); // fill the list
            
        }
        if (enemyObject.TryGetComponent(out EnemyBehaviour enemyBehaviour))
        {
            enemyBehaviour.speed = enemyData.speed;
            enemyBehaviour.power = enemyData.power;
        }
    }
}
