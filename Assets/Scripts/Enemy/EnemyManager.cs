using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;


public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> EnemyDataList = new List<EnemyData>();
    public int NumberOfEnemiesToSpawn = 5;
    int roomIndex;
    private List<List<EnemyHealth>> enemyHealthLists = new List<List<EnemyHealth>>();
    private GameObject enemyHolder;
    StageGenerator stageGenerator;
    EnemySpawner spawner;
    public static AudioSource audioSource; // static so it can be accessed from enemy scripts
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

                Vector3 pos = new Vector3(stageGenerator.roomsList[i].x * stageGenerator.roomWidth, stageGenerator.roomsList[i].y * stageGenerator.roomHeight);
                spawner.BeginProccess(EnemyDataList, NumberOfEnemiesToSpawn, pos, r); // EnemyHolder object ---child---> r
                enemyHealthLists.Add(spawner.enemyHealthList);
                
                //Debug.Log($"Room {i}, enemy count: {enemyHealthLists[i].Count}");
            }
        }
        else
        {
            Vector3 pos = new Vector3 (-30, 5, 0);
            Debug.Log("Stage Generator Not Found!");
            spawner.BeginProccess(EnemyDataList, NumberOfEnemiesToSpawn, pos, enemyHolder);
            enemyHealthLists.Add(spawner.enemyHealthList);
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
    }
    
}
