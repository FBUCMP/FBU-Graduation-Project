using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBackground : MonoBehaviour
{
    // creates a background mesh just like the room, but with a different color
    // does not have any collider or anything, just a background
    // and creates a sprite with a flat color


    public Color bgColor;
    public Sprite sprite;
    public Material colorMaterial;
    public Material meshMaterial;
    private RoomGenerator roomGenerator;
    private MeshGenerator meshGenerator;
    private SpriteRenderer spriteRenderer;
    private GameObject bg; // background

    private float[,] map;
    private int width;
    private int height;
    void Start()
    {
        roomGenerator = GetComponent<RoomGenerator>();

        bg = new GameObject("Background"); // create background object
        bg.transform.position = transform.position;
        bg.transform.parent = transform;

        GameObject spriteHolder = new GameObject("SpriteHolder"); // create another object sprite holder, for flat color
        spriteHolder.transform.position = transform.position;
        spriteHolder.transform.parent = bg.transform;
        spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.material = colorMaterial;
        spriteRenderer.sortingOrder = -2;
        spriteHolder.transform.localScale = new Vector3(roomGenerator.width, roomGenerator.height, 1);
        spriteRenderer.color = bgColor;
        MeshRenderer bgRenderer = bg.AddComponent<MeshRenderer>();
        MeshFilter bgFilter = bg.AddComponent<MeshFilter>();
        bgRenderer.material = meshMaterial;
        bgRenderer.sortingOrder = -1;
        bgRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        meshGenerator = bg.AddComponent<MeshGenerator>();
        GenerateBackgroundMap();
        for (int i = 0; i < 5; i++) 
        {
            SmoothMap();
        }
        meshGenerator.GenerateMeshOnly(map, 2);


    }
    private float[,] GenerateBackgroundMap()
    {
        width = 1+roomGenerator.width / 2; // +1 for the borders
        height = 1+roomGenerator.height / 2;
        map = new float[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) // odanin en dis sinirlari kesin duvar yap
                {
                    map[x, y] = 1f;
                }
                else
                {
                    //(x / (float)width * perlinVals.x) + pseudoRandom.Next(0, 100), 
                    //(y / (float)height * perlinVals.y) + pseudoRandom.Next(0, 100)
                    float val = Mathf.PerlinNoise(
                                                    (x / (float)width * roomGenerator.perlinVals.x/12) + Random.Range(0, 100),
                                                    (y / (float)height * roomGenerator.perlinVals.y/12) + Random.Range(0, 100)
                                                );
                    val = Mathf.Clamp(val, 0f, 1f);
                    map[x, y] = val;
                }
            }
        }
        return map;
    }
    void SmoothMap() // rastgelelik duzgun hale getiriliyor
    {
        int neighbourThreshold = 4; // 4 is the best
        float[,] newMap = (float[,])map.Clone();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y); // komsu duvar sayisi
                if (neighbourWallTiles > neighbourThreshold) // komsu duvar sayisi thresholddan fazla ise duvar yap
                {
                    newMap[x, y] += 0.5f;
                }

                else if (neighbourWallTiles < neighbourThreshold) // komsu duvar sayisi threshholddan az ise bosluk yap
                {
                    newMap[x, y] -= 0.5f;
                }
                newMap[x, y] = Mathf.Clamp(newMap[x, y], 0f, 1f);
            }
        }
        
        map = newMap;
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY] > 0.5f ? 1 : 0;
                    }
                }
                else
                {
                    wallCount++; /* aslinda burada sayim yanlis yapiliyor. egre sinir disi checklenirse duvar varmis gibi davraniliyor boylelikle dislara dogru duvar yogunlugu artacak*/
                }
            }
        }

        return wallCount;
    }
}
