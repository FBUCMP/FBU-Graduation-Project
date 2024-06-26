using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;


public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> EnemyDataList = new List<EnemyData>();
    public int NumberOfEnemiesToSpawn = 5;
    int roomIndex;
    private List<List<EnemyHealth>> enemyHealthLists = new List<List<EnemyHealth>>(); // list of lists of enemy health scripts
    private GameObject enemyHolder; // parent object for all enemies
    StageGenerator stageGenerator;
    EnemySpawner spawner;
    public static AudioSource audioSource; // static so it can be accessed from enemy scripts

    public delegate void RoomCleared(int roomIndex);
    public static event RoomCleared OnRoomCleared; // event to be invoked when all enemies in a room are dead

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.3f;
        stageGenerator = GameObject.FindAnyObjectByType<StageGenerator>();
        spawner = GetComponentInChildren<EnemySpawner>();
        enemyHolder = new GameObject("EnemyHolder");
        CreateEnemy(EnemyDataList);

    }


    public void CreateEnemy(List<EnemyData> enemyData)
    {
        if(stageGenerator !=  null)
        {
            for (int i = 0; i < stageGenerator.roomsList.Count; i++)
            {
                GameObject r = new GameObject("Room " + i);
                r.transform.parent = enemyHolder.transform;

                spawner.BeginProccess(EnemyDataList, NumberOfEnemiesToSpawn, stageGenerator.roomObjects[i], r); // EnemyHolder object ---child---> r
                enemyHealthLists.Add(spawner.enemyHealthList);
                
                //Debug.Log($"Room {i}, enemy count: {enemyHealthLists[i].Count}");
            }
        }
        
        foreach (var list in enemyHealthLists)
        {
            foreach (var health in list)
            {
                health.OnDeath += EnemyDeath;
            }
        }
    }
   
    private void OnDisable()
    {
        foreach (var list in enemyHealthLists)
        {
            foreach (var health in list)
            {
                health.OnDeath -= EnemyDeath;
            }
        }
    }
    private void EnemyDeath(Vector3 pos)
    {
        int room = 0;
        for(int i = 0; i < enemyHealthLists.Count; i++)
        {           
            for(int j = 0; j < enemyHealthLists[i].Count; j++)
            {
                if (enemyHealthLists[i][j].transform.position == pos)
                {
                    enemyHealthLists[i].Remove(enemyHealthLists[i][j]);
                    room = i;
                    break;
                }
            }
        }
        Debug.Log($"Room{room}: {enemyHealthLists[room].Count} enemies left");
        if (enemyHealthLists[room].Count == 0)
        {
            Debug.Log($"Room{room} cleared");
            OnRoomCleared?.Invoke(room);
        }
    }
    
    public void ActivateEnemies(Vector2Int roomCoord)
    {
        Debug.Log("Activate enemies in room: " + roomCoord);
        roomIndex = stageGenerator.roomsList.IndexOf(roomCoord);
        if (roomIndex != -1)
        {
            foreach (Transform child in enemyHolder.transform)
            {
                Debug.Log("Deactivating room: " + child.name);
                child.gameObject.SetActive(false);
            }
            Debug.Log("Activating room: " + enemyHolder.transform.GetChild(roomIndex).name);
            enemyHolder.transform.GetChild(roomIndex).gameObject.SetActive(true);
        }
    }
}
