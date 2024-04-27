using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StageGenerator : MonoBehaviour
{
    [Header("------- Stage Data ------------")]
    public int width = 10;  
    public int height = 10;
    public int maxRooms = 20;
    public int minRooms = 6;
    [Range(0, 1)]
    public float randomness;
    public int stageDifficulty = 1;

    /*
    public Tilemap tilemap;
    public TileBase emptyRoomTile;
    public TileBase filledRoomTile;
    public TileBase startRoomTile;
    public TileBase chestRoomTile;
    public TileBase bossRoomTile;
    public TileBase selectedRoomTile;
    */
    [Header("------- Room Prefab ------------")]
    public GameObject roomPrefab;
    private int squareSize = 1;
    [HideInInspector] public int roomWidth; // room prefabin boyutlari
    [HideInInspector] public int roomHeight;

    [HideInInspector] public List<bool[]> gatesList; // bool[] = 0: up, 1: right, 2: down, 3: left
    [HideInInspector] public Dictionary<Vector2Int, Vector3[]> tpPoints = new Dictionary<Vector2Int, Vector3[]>(); // room kordinatlari ve oda kordinatlarina gore kapi kordinatlari

    //[Header("------- Player Prefab ------------")]
    private GameObject playerPrefab;
    private GameObject player;

    private int[,] roomData; /* orn:
                              * 0 0 1 0 2 0 0 0 0 0
                              * 0 0 1 1 1 1 0 0 0 0
                              * 0 0 0 1 0 1 1 1 0 0
                              * 0 0 1 1 0 1 0 0 0 0
                              * 0 0 1 0 0 1 1 0 0 0
                              * 0 0 4 0 0 1 0 0 0 0
                              * 0 0 0 0 0 0 0 0 0 0
                              * 0 0 0 0 0 0 0 0 0 0
                              * 0 0 0 0 0 0 0 0 0 0
                              * 0 0 0 0 0 0 0 0 0 0
                              * 
                              * 
                              */



    private List<Vector2Int> tempQueue; // ustunde calisilan odayi aliyor is bitince siliyor

    [HideInInspector] public List<Vector2Int> roomsList; /* olusturulan tum odalarin kordinatlari
                                         * orn: [4,9], [4,8], [3,8], [2,8], [4,7] ...
                                         */

    [HideInInspector] public List<GameObject> roomObjects = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        
        if (!roomPrefab && Resources.Load<GameObject>("Prefabs/RandomRoom"))
        {
            roomPrefab = Resources.Load<GameObject>("Prefabs/RandomRoom"); // room prefabini yukle

        }
        if (!playerPrefab && Resources.Load<GameObject>("Prefabs/Player 3"))
        {
            playerPrefab = Resources.Load<GameObject>("Prefabs/Player 3"); // player prefabini yukle

        }
        RoomGenerator prefabsGen = roomPrefab.GetComponent<RoomGenerator>();
        roomWidth = prefabsGen.width + prefabsGen.borderSize ;
        roomHeight = prefabsGen.height + prefabsGen.borderSize;
        

        Init();

        { // scope

            int minY = int.MaxValue;
            int minX = int.MaxValue;
            int maxY = int.MinValue;
            int maxX = int.MinValue;

            for (int y = 0; y < roomData.GetLength(0); y++)
            {
                for (int x = 0; x < roomData.GetLength(1); x++)
                {
                    // Check if the element is non-zero (represents a room)
                    if (roomData[y, x] != 0)
                    {
                        // Update min and max y coordinates
                        minY = Mathf.Min(minY, y);
                        maxY = Mathf.Max(maxY, y);

                        // Update min and max x coordinates
                        minX = Mathf.Min(minX, x);
                        maxX = Mathf.Max(maxX, x);
                    }
                }
            }
            // x and y are reverse
            Vector3 midPoint = new Vector3(((minY + maxY) * roomWidth / 2) , ((minX + maxX) * roomHeight / 2) , 0);
            AstarPath.active.data.gridGraph.center = midPoint; // set the center of the grid graph to the midpoint of the rooms
            float astarNodeSize = 0.25f;
            float dimentionWidth = (roomWidth * (maxY - minY) + roomWidth) / astarNodeSize;
            float dimentionDepth = (roomHeight * (maxX - minX) + roomHeight) / astarNodeSize;
            AstarPath.active.data.gridGraph.SetDimensions(Mathf.CeilToInt( dimentionWidth), Mathf.CeilToInt(dimentionDepth), astarNodeSize); // set the dimensions of the grid graph to the size of the rooms combined
            
            AstarPath.active.Scan();
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Y)) 
        //{
        //    Init();
        //}
        
    }

    void Init()
    {
        InitializeArray();
        GenerateStage();
        PlaceRooms();
        PlacePlayer();
        
        //Debug.Log($"Starting: ({roomsList[0].x},{roomsList[0].y}) | Boss: ({roomsList[roomsList.Count - 1].x},{roomsList[roomsList.Count - 1].y}) | RoomCount: {roomsList.Count}");
    }
    void PlacePlayer(int roomIndex, Vector2 position)
    {
        if(roomIndex > roomsList.Count)
        {
            Debug.Log($"Cannot place player at index: {roomIndex}, bounds exceeded");
            return;
        }
        if (GameObject.FindGameObjectWithTag("Player")) // player varsa sil
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
        player = Instantiate(playerPrefab, new Vector3((roomsList[roomIndex].x * roomWidth) - (roomWidth/2) + position.x, (roomsList[roomIndex].y * roomHeight) - (roomHeight/2) + position.y, 0), Quaternion.identity); // prefabdan player instance olustur
        Debug.Log("Player Placed at: "+ new Vector3(roomsList[roomIndex].x * roomWidth, roomsList[roomIndex].y * roomHeight, 0));

    }
    void PlacePlayer(int roomIndex = 0)
    {

        if (GameObject.FindGameObjectWithTag("Player")) // player varsa sil
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
        player = Instantiate(playerPrefab, new Vector3((roomsList[roomIndex].x * roomWidth), (roomsList[roomIndex].y * roomHeight), 0), Quaternion.identity); // prefabdan player instance olustur
        roomObjects[roomIndex].GetComponent<HeatMap>().UpdateHeatMap(new Bounds(player.transform.position,new Vector3(3,3)));
        Debug.Log("Player Placed at: " + new Vector3(roomsList[roomIndex].x * roomWidth, roomsList[roomIndex].y * roomHeight, 0));

    }


    private void OnDrawGizmos()
    {
        // draw 4 lines around the area that rooms can be placed
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(0, 0, 0) + new Vector3(- roomWidth/2, - roomHeight/2, 0);
        Vector3 bottomRight = new Vector3(width * roomWidth, 0, 0) + new Vector3(roomWidth / 2, -roomHeight / 2, 0);
        Vector3 topLeft = new Vector3(0, height * roomHeight, 0) + new Vector3(-roomWidth / 2, roomHeight / 2, 0);
        Vector3 topRight = new Vector3(width * roomWidth, height * roomHeight, 0) + new Vector3(roomWidth / 2, roomHeight / 2, 0);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);

    }
    void CalculateConnectedSides() // connected rooms
    {
        gatesList = new List<bool[]>(); // 0: up, 1: right, 2: down, 3: left
        for (int i = 0; i < roomsList.Count; i++)
        {
            bool[] connectedSides = new bool[4];
            Vector2Int room = roomsList[i];
            Vector3[] tpPointsArray = new Vector3[4];
            float offset = 8f;
            // notice that the order is different than the gatesList to match the direction of the gates
            tpPointsArray[2] = new Vector3(room.x * roomWidth, (room.y * roomHeight + roomHeight / 2) - offset, 0); // up
            tpPointsArray[3] = new Vector3( (room.x * roomWidth + roomWidth / 2) - offset, room.y * roomHeight, 0); // right
            tpPointsArray[0] = new Vector3(room.x * roomWidth, (room.y * roomHeight - roomHeight / 2) + offset, 0); // down
            tpPointsArray[1] = new Vector3( (room.x * roomWidth - roomWidth / 2) + offset, room.y * roomHeight, 0); // left
            if(roomsList.Contains(new Vector2Int(room.x, room.y + 1))) // up
            {
                connectedSides[0] = true;
            }
            if (roomsList.Contains(new Vector2Int(room.x + 1, room.y))) // right
            {
                connectedSides[1] = true;
            }
            if (roomsList.Contains(new Vector2Int(room.x, room.y - 1))) // down
            {
                connectedSides[2] = true;
            }
            if (roomsList.Contains(new Vector2Int(room.x - 1, room.y))) // left
            {
                connectedSides[3] = true;
            }
            gatesList.Add(connectedSides); // all bools have all true values
            if (tpPoints.ContainsKey(room))
            {
                tpPoints[room] = tpPointsArray;
            }
            else
            { 
                tpPoints.Add(room, tpPointsArray);
            }
            //Debug.Log(room);
        }

    }

    void InitializeArray() // odanin arrayini once 0larla doldur
    {
        roomData = new int[width, height];
        tempQueue = new List<Vector2Int>();
        roomsList = new List<Vector2Int>();

        gatesList = new List<bool[]>();
        tpPoints = new Dictionary<Vector2Int, Vector3[]>();

        // fill roomData with 0
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                roomData[x, y] = 0;
            }
        }
    }

    void GenerateStage() // odalarin olacagi konumu hesaplayan algoritma
    {
        while (roomsList.Count < minRooms)
        {
            InitializeArray();
            int startX = (int)(width / 2); //Random.Range(0, width);
            int startY = (int)height -1;//(int)(height / 2); //Random.Range(0, height);
            Vector2Int startCell = new Vector2Int(startX, startY);

            //first room manually added
            //tempQueue.Add(startCell);
            roomsList.Add(startCell); 
            roomData[startX, startY] = 1;
            startCell.y--;
            
            //second room manually added to below the first room
            roomData[startX, startY] = 1;
            tempQueue.Add(startCell); 
            roomsList.Add(startCell); 
            int generatedRooms = 2;
            //Debug.Log($"Room Generated at: {startX},{startY}");
            while (tempQueue.Count > 0 && generatedRooms < maxRooms)
            {

                Vector2Int currentCell = tempQueue[0];
                tempQueue.RemoveAt(0);
            

                Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighborCell = currentCell + direction;

                    if (IsValidCell(neighborCell) && roomData[neighborCell.x, neighborCell.y] == 0)
                    {
                        int neighborRoomCount = CountFilledNeighbors(neighborCell); // komsu cell'leri say (caprazlar yok !)

                        if (neighborRoomCount == 1 && (Random.Range(0f, 1f) < randomness )) // rastgelelik var randomness 0.5 ise %50 ihtimal. komsu sayisi 1se
                        {
                            if (!roomsList.Contains(neighborCell))
                            {
                                roomData[neighborCell.x, neighborCell.y] = 1; //burada secilen oda 0 -> 1 e degistiriliyor
                                tempQueue.Add(neighborCell); // tempQueue alinip siliyor gecici
                                roomsList.Add(neighborCell); // roomsList bastan olusma sirasina gore ekliyor ve kalici. oda kordinatlari
                                generatedRooms++;
                                //Debug.Log($"Room Generated at: {neighborCell.x},{neighborCell.y}");
                            }
                        }

                    }
                }
            }


            // room cesitlerinin numaralari roomDatada duzenlenir
            if (roomsList.Count > 0)
            {
                roomData[roomsList[0].x, roomsList[0].y] = 2; // start room 2
                                                               //chest room 3 
                roomData[roomsList[roomsList.Count - 1].x, roomsList[roomsList.Count - 1].y] = 4; // boss room 4
            }
        }
    }

    bool IsValidCell(Vector2Int cell) // sinirlarin disinda mi degil mi
    {
        return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
    }

    int CountFilledNeighbors(Vector2Int cell) // komsu cell'leri say (caprazlar yok !)
    {
        int count = 0;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions) // sirayla etrafina bak
        {
            Vector2Int neighborCell = cell + direction;

            if (IsValidCell(neighborCell) && roomData[neighborCell.x, neighborCell.y] != 0)
            {
                count++;
            }
        }

        return count;
    }

    void PlaceRooms()
    {
        CalculateConnectedSides();
        foreach (Transform child in transform) // onceki odalari sil
        {
            Destroy(child.gameObject);
        }
        foreach (Vector2Int roomCoord in roomsList) // odalari yerlestir
        {
            GameObject spawnedRoom = (GameObject)Instantiate(roomPrefab, new Vector3(roomCoord.x * roomWidth, roomCoord.y * roomHeight,0), Quaternion.identity);
            if (spawnedRoom.GetComponent<RoomGenerator>())
            {
                RoomGenerator roomsGen = spawnedRoom.GetComponent<RoomGenerator>();
                roomsGen.roomIndex = roomsList.IndexOf(roomCoord); 
                roomsGen.gates = gatesList[roomsList.IndexOf(roomCoord)];
                roomsGen.difficulty = stageDifficulty + Random.Range(0,3);
                roomsGen.squareSize = squareSize;
                // // can do changes to the first generated room HERE with roomsList.IndexOf(roomCoord) == 0
                
                roomsGen.GenerateMap(); // generate mapi elle cagiriyoruz.



            }
            spawnedRoom.transform.parent = gameObject.transform; // her bir eklenen oda stage generatorun childi oluyor
            
            spawnedRoom.name = $"Room{roomsList.IndexOf(roomCoord)}";
            roomObjects.Add(spawnedRoom);
            
        }
        
    }

}
