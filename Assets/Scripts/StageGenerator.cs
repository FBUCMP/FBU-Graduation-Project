using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class StageGenerator : MonoBehaviour
{
    public int width = 10;  
    public int height = 10;
    public int maxRooms = 20;
    public int minRooms = 6;
    [Range(0, 1)]
    public float randomness;

    public Tilemap tilemap;
    public TileBase emptyRoomTile;
    public TileBase filledRoomTile;
    public TileBase startRoomTile;
    public TileBase chestRoomTile;
    public TileBase bossRoomTile;
    public TileBase selectedRoomTile;

    private int[,] roomArray;
    private List<Vector2Int> roomQueue;
    private List<Vector2Int> roomsList;

    // Start is called before the first frame update
    void Start()
    {
        InitializeArray();
        GenerateRooms();
        UpdateTilemap();
        StartCoroutine(VisualizeTiles());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) // space'e basilirsa
        {
            StopCoroutine(VisualizeTiles());
            InitializeArray();
            GenerateRooms();
            UpdateTilemap();
            StartCoroutine(VisualizeTiles());
            foreach (Vector2Int room in roomsList)
            {
                Debug.Log(room);
            }
        }
    }

    void InitializeArray() // odanin arrayini once 0larla doldur
    {
        roomArray = new int[width, height];
        roomQueue = new List<Vector2Int>();
        roomsList = new List<Vector2Int>();
        // fill roomArray with 0
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                roomArray[x, y] = 0;
            }
        }
    }

    void GenerateRooms() // asil odalari secen algoritma
    {
        int startX = Random.Range(0, width);
        int startY = Random.Range(0, height);
        Vector2Int startCell = new Vector2Int(startX, startY);

        roomQueue.Add(startCell);
        roomsList.Add(startCell);
        roomArray[startX, startY] = 1;
        int generatedRooms = 1;

        while (roomQueue.Count > 0 && generatedRooms < maxRooms)
        {

            Vector2Int currentCell = roomQueue[0];
            roomQueue.RemoveAt(0);
            

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighborCell = currentCell + direction;

                if (IsValidCell(neighborCell) && roomArray[neighborCell.x, neighborCell.y] == 0)
                {
                    int neighborRoomCount = CountFilledNeighbors(neighborCell);

                    if (neighborRoomCount == 1 && (Random.Range(0f, 1f) < randomness || generatedRooms < minRooms)) // rastgelelik var randomness 0.5 ise %50 ihtimal
                    {
                        roomArray[neighborCell.x, neighborCell.y] = 1; //burada secilen oda 0 -> 1 e degistiriliyor
                        roomQueue.Add(neighborCell);
                        roomsList.Add(neighborCell);
                        generatedRooms++;
                    }

                }
            }
        }
        // room cesitlerinin numaralari roomArrayde duzenlenir
        if (roomsList.Count > 0)
        {
            roomArray[roomsList[0].x, roomsList[0].y] = 2; // start room 2
                                                           //chest room 3 
            roomArray[roomsList[roomsList.Count - 1].x, roomsList[roomsList.Count - 1].y] = 4; // boss room 4
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

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCell = cell + direction;

            if (IsValidCell(neighborCell) && roomArray[neighborCell.x, neighborCell.y] != 0)
            {
                count++;
            }
        }

        return count;
    }

    
    void UpdateTilemap()// tile cizimleri burada
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tile;
                switch (roomArray[x, y])
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
}
