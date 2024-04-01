using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyData> EnemyDataList = new List<EnemyData>();
    public int NumberOfEnemiesToSpawn = 5;
    int roomIndex;

    StageGenerator stageGenerator;
    EnemySpawner spawner;
    public static AudioSource audioSource; // static so it can be accessed from enemy scripts
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.3f;
        stageGenerator = GameObject.FindAnyObjectByType<StageGenerator>();
        spawner = GetComponentInChildren<EnemySpawner>();
        CreateEnemy(EnemyDataList);

    }


    public void CreateEnemy(List<EnemyData> enemyData)
    {
        if(stageGenerator !=  null)
        {
            for (int i = 0; i < stageGenerator.roomsList.Count; i++)
            {
                Vector3 pos = new Vector3(stageGenerator.roomsList[i].x * stageGenerator.roomWidth, stageGenerator.roomsList[i].y * stageGenerator.roomHeight);
                spawner.BeginProccess(EnemyDataList, NumberOfEnemiesToSpawn, pos);
            }
        }
        else
        {
            Vector3 pos = new Vector3 (-30, 5, 0);
            Debug.Log("Stage Generator Not Found!");
            spawner.BeginProccess(EnemyDataList, NumberOfEnemiesToSpawn, pos);
        }
       
    }

}
