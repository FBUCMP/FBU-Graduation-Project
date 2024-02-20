using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// TODO: folderda file varsa o file i ac devam ettir. isim default
public class DrawRoom : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private GameObject canvas; // square gameobjecti surukle birak
    [SerializeField] public string roomName = "Room_0";
    private string roomJSONsFolderName = "RoomJSONs";

    public float[,] map; // 0-1 li data. amac bunu degistirmek. mesh icin kullanýlan tek veri bu
    private int brushSize = 0;
    private MeshGenerator meshGenerator;
    private void Start()
    {
        map = new float[width, height];
        meshGenerator = GetComponent<MeshGenerator>();
        meshGenerator.GenerateMesh(map, 1);
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = height/2; // cameranin boyutunu cizilen alana gore ayarla
        }

        
        canvas.transform.localScale = new Vector3(width, height, 0); /* beyaz squarein boyunu ayarla tamamen gorsellik icin baska islevi yok*/
        if (roomName.Trim() != "")
        {
            OpenRoom(roomName);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // sol tik basili tutuldugunda
        {
            HandleMouseInput(1);
        }
        else if (Input.GetMouseButton(1)) // sag tik basili
        {
            HandleMouseInput(0);
        }

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            SaveRoom(roomName);
        }
        HandleBrush();
    }

    private void HandleBrush()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            brushSize += 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            if (brushSize > 0)
            {
                brushSize -= 1;
            }
        }
    }

    void OpenRoom(string fileName)
    {
        Debug.Log("openroom");
        if (!Directory.Exists(Path.Combine(Application.dataPath, roomJSONsFolderName))) // if the folder for room jsons doesnt exist
        {
            Debug.Log("Error on OpenRoom. Directory not found!");
            return;
        }
        string fullPath = Path.Combine(Application.dataPath, roomJSONsFolderName, fileName + ".json"); // Assets/RoomJSONs/Room_0.json
        Debug.Log(fullPath);

        
        if (File.Exists(fullPath)) // if a file with fileName exists
        {
            
            string json = File.ReadAllText(fullPath);
            Room loadedRoom = JsonUtility.FromJson<Room>(json); // loaded room istenilen
            map = OneDimentionToTwo(loadedRoom.map, loadedRoom.width, loadedRoom.height) ;
            meshGenerator.GenerateMesh(map, 1);
        

        }
        
    }

    private void SaveRoom(string fileName) // eksik
        
    {
        if (!Directory.Exists(Path.Combine(Application.dataPath, roomJSONsFolderName))) // if the folder for room jsons doesnt exist
        {
            Debug.Log("Error on SaveRoom. Directory not found!");
            return;
        }
        int fileCount = Directory.GetFiles(Path.Combine(Application.dataPath, roomJSONsFolderName), "*.json").Length;
        if (fileName.Trim() == "")
        {
            fileName = "Room_" + fileCount.ToString();
        }
        string fullPath = Path.Combine(Application.dataPath, roomJSONsFolderName, fileName + ".json");
        Room room = new Room(TwoDimentionToOne(map), width, height);

        string json = JsonUtility.ToJson(room);
        File.WriteAllText(fullPath, json);
    }
    private void HandleMouseInput(int value)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // tiklanan noktanin dunyadaki kordinati
        Vector2Int cellCoords = new Vector2Int(Mathf.FloorToInt(mousePos.x + width/2), Mathf.FloorToInt(mousePos.y + height/2)); // kordinattan arraye uygun indexe cevir

        if (IsWithinGridBounds(cellCoords)) // alanin icinde ise
        {
            
            DrawCicle(cellCoords, value, brushSize); // cizim burda
            
            meshGenerator.GenerateMesh(map, 1); // mesh generator scriptine erisim. 0-1 datayi gorsellestiriyor
        }

    }
    private void DrawCicle(Vector2Int coord, int value, int r) {
        if (r==0) // brush boyutu en ufaksa direkt ciz
        {
            map[coord.x, coord.y] = value;
        }
        else // brush boyu daha buyukse daire seklinde buyut
        {
            for (int x = -r; x <= r; x++) // for lar daire sekli icin
            {
                for (int y = -r; y < r; y++)
                {
                    if (x*x + y*y <= r*r)
                    {
                        int drawX = coord.x + x;
                        int drawY = coord.y + y;
                        if (IsWithinGridBounds(new Vector2Int(drawX, drawY)))
                        {
                            map[drawX, drawY] = value;
                        }
                    }
                }
            }

        }
    }
    private bool IsWithinGridBounds(Vector2Int cellCoords) // alanin disina tasmamasi icin
    {
        return cellCoords.x >= 0 && cellCoords.x < width && cellCoords.y >= 0 && cellCoords.y < height;
    }

    private float[,] OneDimentionToTwo(float[] arr, int width, int height) // 1D arrayi 2D yap
    {
        if (arr.Length != width * height)
        {
            Debug.LogError("Input array size does not match the given width and height.");
            return null;
        }

        float[,] result = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + y * width;
                result[x, y] = arr[index];
            }
        }

        return result;
    }

    private float[] TwoDimentionToOne(float[,] arr) // 2D arrayi 1D yap
    {
        int width = arr.GetLength(0);
        int height = arr.GetLength(1);

        float[] result = new float[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + y * width;
                result[index] = arr[x, y];
            }
        }

        return result;
    }

}
