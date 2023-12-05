using UnityEngine;
using System.Collections;
using System;

public class RoomGenerator : MonoBehaviour
{

	/*
	 * room generator zorluk vb datayi da tutabilir. room prefableri kaydederiz. random rooom icin 1 prefab,
	 * her bir boss room icin ayri ayi prefableri olustururuz.
	 * yani room jsonunu okuyan kisim olustur. room jsonu yoksa random olsun.
	 */

	public int width = 50;
	public int height = 30;
	public int roomIndex = 0;
	public int difficulty = 1;
	public int borderSize = 1;
	public bool[] gates = new bool[4]; // 0: up, 1: right, 2: down, 3: left
	public GameObject gatePrefab;

	[Range(0,10)]
	public int smoothness;
	public string seed;
	public bool useRandomSeed = true;

	[Range(0, 100)]
	public int randomFillPercent;

	int[,] map; // bu script map arrayini 0-1 ler ile dolduruyo
	void Start()
	{
        
		gatePrefab = Resources.Load<GameObject>("Prefabs/Gate"); // gate prefabini yukle
		
        
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			GenerateMap();
		}
	}


	public void GenerateMap() // bu bir room datasi olusturuyor
	{
		map = new int[width, height];
		RandomFillMap(); // indexe bagli rastgelelikle doldur

		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
		}

		int[,] borderedMap = new int[width + (borderSize * 2), height + (borderSize * 2)];
		for (int x = 0; x < borderedMap.GetLength(0); x++)
		{
			for (int y = 0; y < borderedMap.GetLength(1); y++)
			{
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
					borderedMap[x, y] = map[x - borderSize, y - borderSize];
                }
                else
                {
					borderedMap[x, y] = 1;
                }
			}
		}
		
		
        // 0: up, 1: right, 2: down, 3: left
		int platformLength = 7;
		int gateSpaceSize = 7;
		float gateDistanceToWall = 1.25f;
		Vector3 roomBottomLeft = new Vector3(transform.position.x - (width/2), transform.position.y - (height/2), 0);
        if (gates[0]) // up
        {
            DrawCicle(borderedMap, new Vector2Int(width / 2, height - 1), 0, gateSpaceSize);
            DrawCicle(borderedMap, new Vector2Int(width / 2, height - 3), 0, gateSpaceSize);
			for (int i = 0; i < platformLength; i++)
			{
                DrawCicle(borderedMap, new Vector2Int((width / 2) - ((int)(platformLength/2)) + i , height - gateSpaceSize), 1, 1);
            }
			GameObject g0 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3(width / 2, height - (gateDistanceToWall - borderSize), 0), Quaternion.Euler(0,0,90));
			g0.transform.parent = transform;

        }
        if (gates[1]) // right
        {
            DrawCicle(borderedMap, new Vector2Int(width - 1, height / 2), 0, gateSpaceSize);
            DrawCicle(borderedMap, new Vector2Int(width - 3, height / 2), 0, gateSpaceSize);
            for (int i = 0; i < platformLength; i++)
            {
                DrawCicle(borderedMap, new Vector2Int((width)-i, (height / 2) - gateSpaceSize), 1, 1);
            }
            GameObject g1 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3(width - (gateDistanceToWall - borderSize), height / 2, 0), Quaternion.identity);
            g1.transform.parent = transform;

        }
        if (gates[2]) // down
        {
            DrawCicle(borderedMap, new Vector2Int(width / 2, 0), 0, gateSpaceSize);
            DrawCicle(borderedMap, new Vector2Int(width / 2, 2), 0, gateSpaceSize);
            for (int i = 0; i < platformLength; i++)
            {
                DrawCicle(borderedMap, new Vector2Int((width / 2) - ((int)(platformLength / 2)) + i, gateSpaceSize), 1, 1);
            }
            GameObject g2 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3(width / 2, (gateDistanceToWall - borderSize), 0), Quaternion.Euler(0, 0, -90));
            g2.transform.parent = transform;
        }
        if (gates[3]) // left
        {
            DrawCicle(borderedMap, new Vector2Int(0, height / 2), 0, gateSpaceSize);
            DrawCicle(borderedMap, new Vector2Int(2, height / 2), 0, gateSpaceSize);
			for(int i = 0;i < platformLength; i++)
			{
				DrawCicle(borderedMap, new Vector2Int(1+i, (height / 2) - gateSpaceSize), 1, 1);
			}
            GameObject g3 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3((gateDistanceToWall - borderSize), height / 2, 0), Quaternion.Euler(0, 0, 180));
            g3.transform.parent = transform;
        }
		
        
				
		MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
		meshGenerator.GenerateMesh(borderedMap, 1f); // MeshGenerator scriptinden fonksiyon. map gonderiliyor.
	}


	void RandomFillMap()
	{
		if (useRandomSeed)
		{
			
			seed = Time.time.ToString() + roomIndex.ToString(); // time random icin, aynanda olusan odalar farkli olsun diye index eklendi
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) // odanin en dis sinirlari kesin duvar yap
				{
					map[x, y] = 1;
				}
				else
				{
					map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0; // kalani rastgele 1 veya 0 ver random noise üret
				}
			}
		}
	}

	void SmoothMap() // rastgelelik duzgun hale getiriliyor
	{

		int[,] newMap = (int[,])map.Clone(); 
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(x, y); // komsu duvar sayisi

				if (neighbourWallTiles > 4)	// komsu duvar sayisi 4ten fazla ise duvar yap
					newMap[x, y] = 1;
					
				else if (neighbourWallTiles < 4) // komsu duvar sayisi 4ten az ise bosluk yap
					newMap[x, y] = 0;
					
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
						wallCount += map[neighbourX, neighbourY];
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

	

    private void DrawCicle(int[,] _map , Vector2Int coord, int value, int r)
    {
        if (r == 0) // brush boyutu en ufaksa direkt ciz
        {
            _map[coord.x, coord.y] = value;
        }
        else // brush boyu daha buyukse daire seklinde buyut
        {
            for (int x = -r; x <= r; x++) // for lar daire sekli icin
            {
                for (int y = -r; y < r; y++)
                {
                    if (x * x + y * y <= r * r)
                    {
                        int drawX = coord.x + x;
                        int drawY = coord.y + y;
                        if (IsWithinGridBounds(_map, new Vector2Int(drawX, drawY)))
                        {
                            _map[drawX, drawY] = value;
                        }
                    }
                }
            }

        }
    }
    private bool IsWithinGridBounds(int[,] _map, Vector2Int cellCoords) // alanin disina tasmamasi icin
    {
		return cellCoords.x > 0 && cellCoords.x < _map.GetLength(0) -1 && cellCoords.y > 0 && cellCoords.y < _map.GetLength(1) -1;
    }
}