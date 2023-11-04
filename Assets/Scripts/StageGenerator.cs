using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    private int roomWidth; // room prefabin boyutlari
    private int roomHeight;

    [Header("------- Player Prefab ------------")]
    public GameObject playerPrefab;

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

    private List<Vector2Int> roomsList; /* olusturulan tum odalarin kordinatlari
                                         * orn: [4,9], [4,8], [3,8], [2,8], [4,7] ...
                                         */

    // Start is called before the first frame update
    void Awake()
    {
        
        if (!roomPrefab && Resources.Load<GameObject>("Prefabs/RandomRoom"))
        {
            roomPrefab = Resources.Load<GameObject>("Prefabs/RandomRoom"); // room prefabini yukle

        }
        if (!playerPrefab && Resources.Load<GameObject>("Prefabs/Player 1"))
        {
            playerPrefab = Resources.Load<GameObject>("Prefabs/Player 1"); // player prefabini yukle

        }
        roomWidth = roomPrefab.GetComponent<RoomGenerator>().width;
        roomHeight = roomPrefab.GetComponent<RoomGenerator>().height;

        Init();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            Init();
        }
    }

    void Init()
    {
        //StopCoroutine(VisualizeTiles());
        InitializeArray();
        GenerateRooms();
        PlaceRooms();
        PlacePlayer();
        //UpdateTilemap();
        //StartCoroutine(VisualizeTiles());
        Debug.Log($"Starting: ({roomsList[0].x},{roomsList[0].y}) | Boss: ({roomsList[roomsList.Count - 1].x},{roomsList[roomsList.Count - 1].y}) | RoomCount: {roomsList.Count}");
    }
    void PlacePlayer()
    {
        
        if (GameObject.FindGameObjectWithTag("Player")) // player varsa sil
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
        Instantiate(playerPrefab, new Vector3(roomsList[0].x * roomWidth, roomsList[0].y * roomHeight, 0), Quaternion.identity); // prefabdan player instance olustur
        Debug.Log("Player Placed at: "+ new Vector3(roomsList[0].x * roomWidth, roomsList[0].y * roomHeight, 0));

    }
    /*
    
    // devam edilecek
    private void OnDrawGizmos()
    {
        if (roomsList.Count > 0)
        {
            foreach (Vector2Int roomCoord in roomsList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(roomCoord.x * roomWidth, roomCoord.y * roomHeight, 0), new Vector3(10, 10, 0));
            }
        }
    }*/
    void InitializeArray() // odanin arrayini once 0larla doldur
    {
        roomData = new int[width, height];
        tempQueue = new List<Vector2Int>();
        roomsList = new List<Vector2Int>();
        // fill roomData with 0
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                roomData[x, y] = 0;
            }
        }
    }

    void GenerateRooms() // asil odalari secen algoritma
    {
        while (roomsList.Count < minRooms)
        {
            
            int startX = (int)(width / 2); //Random.Range(0, width);
            int startY = (int)height -1;//(int)(height / 2); //Random.Range(0, height);
            Vector2Int startCell = new Vector2Int(startX, startY);

            tempQueue.Add(startCell);
            roomsList.Add(startCell); 
            roomData[startX, startY] = 1;
            int generatedRooms = 1;

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
                            roomData[neighborCell.x, neighborCell.y] = 1; //burada secilen oda 0 -> 1 e degistiriliyor
                            tempQueue.Add(neighborCell); // tempQueue alinip siliyor gecici
                            roomsList.Add(neighborCell); // roomsList bastan olusma sirasina gore ekliyor ve kalici. oda kordinatlari
                            generatedRooms++;
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
                roomsGen.roomIndex = roomsList.IndexOf(roomCoord); // burdan sonra manuel generatelemek gerekebilir



            }
            spawnedRoom.transform.parent = gameObject.transform; // her bir eklenen oda stage generatorun childi oluyor
            
        }
        
    }

    //-----------------------------------sonrasi gereksiz----------------------------------
    /*
    void UpdateTilemap()// tile cizimleri burada
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tile;
                switch (roomData[x, y])
                {
                    case 1:
                        tile = filledRoomTile;
                        break;
                    case 2:
                        tile = startRoomTile;
                        break;
                    case 3:
                        tile = chestRoomTile;
                        break;
                    case 4:
                        tile = bossRoomTile;
                        break;
                    default:
                        tile = emptyRoomTile;
                        break;
                }
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        
        

    }
    IEnumerator VisualizeTiles()
    {
        for (int i = 1; i < roomsList.Count-1; i++) 
        {
            tilemap.SetTile(new Vector3Int(roomsList[i].x, roomsList[i].y, 0), selectedRoomTile);
            yield return new WaitForSeconds(0.5f);
        }
    }

    */
}
