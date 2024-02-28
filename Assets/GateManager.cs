using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    //attached to Stage gameobject
    StageGenerator stageGen;
    List<bool[]> gatesList;
    List<Vector2Int> roomsList;
    private Vector2Int currentRoom;
    Transform player;
    Vector3 newPos;

    public delegate void TeleportEvent(Vector3 roomCenter);
    public static event TeleportEvent OnTeleport;


    void Start()
    {
        stageGen = GetComponent<StageGenerator>();
        gatesList = stageGen.gatesList;
        roomsList = stageGen.roomsList;
        currentRoom = roomsList[0];
        Debug.Log("First Current room: " + currentRoom);
        Gate.OnGateCollide += OnGateCollide;
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
        Debug.Log("OnCollideCurrent room: " + this.currentRoom);
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
        if (stageGen.tpPoints.ContainsKey(room))
        {
            newPos = stageGen.tpPoints[room][dirFrom];
        }
        Debug.Log("Teleported To: " + newPos);
        if (player != null && newPos != null)
        {
            player.position = newPos; // tppoints[room] -> gives array with 4 vectors. 0: down, 1: left, 2: up, 3: right (opposites of gates)
            OnTeleport?.Invoke(new Vector3(currentRoom.x * stageGen.roomWidth , currentRoom.y * stageGen.roomHeight, 0));
        }
    }
}
