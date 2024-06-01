using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateManager : MonoBehaviour
{
    //attached to Stage gameobject
    StageGenerator stageGen;
    List<Vector2Int> roomsList;
    private Vector2Int currentRoom; // current room player is in
    Transform player;
    Vector3 newPos;
    public AudioClip teleportSound;
    private AudioSource audioSource;

    public delegate void TeleportEvent(Vector3 roomCenter);
    public static event TeleportEvent OnTeleport;

    private void Awake()
    {
        Gate.OnGateCollide += OnGateCollide;
        EnemyManager.OnRoomCleared += OnRoomCleared;
    }
    private void OnDestroy()
    {
        Gate.OnGateCollide -= OnGateCollide;
        EnemyManager.OnRoomCleared -= OnRoomCleared;
    }
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.3f;
        stageGen = GetComponent<StageGenerator>();
        roomsList = stageGen.roomsList;
        currentRoom = roomsList[0];
        player = GameObject.FindGameObjectWithTag("Player").transform;

        TeleportTo(currentRoom, 0);

    }
    private void Update()
    {
        //Debug.Log("Current room: " + currentRoom);
    }
    void OnGateCollide(int dir)
    {
        // 0: up, 1: right, 2: down, 3: left
        // set current room according to direction
        if( dir == 0)
        {
            this.currentRoom.y++;
        }
        else if(dir == 1)
        {
            this.currentRoom.x++;
        }
        else if(dir == 2)
        {
            this.currentRoom.y--;
        }
        else if(dir == 3)
        {
            this.currentRoom.x--;
        }
        
        // if current room is the room at the end of the list, change scene
        if (currentRoom == roomsList[roomsList.Count - 1])
        {
            // change scene
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.transform.position = new Vector3(0, 0, 0);
            DontDestroyOnLoad(playerObject);

            SceneManager.LoadSceneAsync(2);
        }
        else
        {
            TeleportTo(this.currentRoom, dir); // teleport player to the new room
            // activate enemies in the new room          
            
            EnemyManager enemyManager = FindObjectOfType<EnemyManager>();// find enemy manager
            if (enemyManager != null)
            {
                Debug.Log("Call: Activate enemies in room: " + currentRoom);
                enemyManager.ActivateEnemies(currentRoom);
            }
        }

    }
    void TeleportTo(Vector2Int room, int dirFrom)
    {
        if (audioSource != null && teleportSound != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }
        if (stageGen.tpPoints.ContainsKey(room))
        {
            newPos = stageGen.tpPoints[room][dirFrom];
        }
        if (player != null && newPos != null)
        {
            player.position = newPos; // tppoints[room] -> gives array with 4 vectors. 0: down, 1: left, 2: up, 3: right (opposites of gates)
            OnTeleport?.Invoke(new Vector3(currentRoom.x * stageGen.roomWidth , currentRoom.y * stageGen.roomHeight, 0));
        }
    }

    void OnRoomCleared(int roomIndex)
    {
        // every gate is set to closed at the beginning of the game
        // when all enemies in a room are dead, open all gates in that room
        Gate[] gates = stageGen.roomObjects[roomIndex].GetComponentsInChildren<Gate>();
        foreach (Gate gate in gates)
        {
            gate.isClosed = false;
            gate.UpdateColor();
        }
    }
}
