using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    //attached to Stage gameobject
    StageGenerator stageGen;
    List<Vector2Int> roomsList;
    private Vector2Int currentRoom;
    Transform player;
    Vector3 newPos;
    public AudioClip teleportSound;
    private AudioSource audioSource;

    public delegate void TeleportEvent(Vector3 roomCenter);
    public static event TeleportEvent OnTeleport;

    private void Awake()
    {
        Gate.OnGateCollide += OnGateCollide;
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
        Debug.Log("Collided with gate, dir: " + dir);
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
        TeleportTo(this.currentRoom, dir);
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
}
