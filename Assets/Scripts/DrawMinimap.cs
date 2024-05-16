using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMinimap : MonoBehaviour
{
    StageGenerator stageGenerator;
    Vector2Int currentRoom;
    void Start()
    {
        stageGenerator = GameObject.FindAnyObjectByType<StageGenerator>(); // there is only one StageGenerator in the scene
        GateManager.OnTeleport += OnTeleport;
        Draw();
    }

    private void OnDisable()
    {
        GateManager.OnTeleport -= OnTeleport;
    }


    void OnTeleport(Vector3 roomCenter)
    {
        currentRoom = new Vector2Int((int)roomCenter.x / stageGenerator.roomWidth, (int)roomCenter.y / stageGenerator.roomHeight);
        Draw();

    }
    void Draw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        int width = stageGenerator.roomData.GetLength(0);
        int height = stageGenerator.roomData.GetLength(1);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (stageGenerator.roomData[i, j] > 0)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.parent = transform;
                    cube.transform.position = new Vector2(i, j) + (Vector2)transform.position + new Vector2(- width/2, - height/1.5f);
                    cube.transform.localScale = new Vector3(1f, 1f, 1f);
                    if (stageGenerator.roomData[i, j] == 1)
                        cube.GetComponent<Renderer>().material.color = Color.white;
                    else if (stageGenerator.roomData[i, j] == 2)
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    else if (stageGenerator.roomData[i, j] == 4)
                        cube.GetComponent<Renderer>().material.color = Color.red;
                    else
                        cube.GetComponent<Renderer>().material.color = Color.yellow; // chestroom maybe later
                    //normalroom 1, startroom 2, chestroom dont exist, bossroom 4
                    
                    if (currentRoom.x == i && currentRoom.y == j)
                    {
                        GameObject playerIcon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        playerIcon.transform.parent = transform;
                        playerIcon.transform.position = new Vector3(i, j) + transform.position + new Vector3(-width / 2, -height / 1.5f, 9);
                        playerIcon.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        playerIcon.GetComponent<Renderer>().material.color = Color.blue;
                    }
                }
            }
        }
    }

}
