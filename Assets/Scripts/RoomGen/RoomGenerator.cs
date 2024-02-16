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

	// TODO: square size gate kýsmýný 0 yapmýyor. bunu duzelt
	// TODO: square size 1den farkliysa collider hatalari olusuyor. bunu duzelt
	public int width = 50;
	public int height = 30;
	public int roomIndex = 0;
	public int squareSize = 1;
	public int difficulty = 1;
	public int borderSize = 1;
	public bool[] gates = new bool[4]; // 0: up, 1: right, 2: down, 3: left
	public GameObject gatePrefab;

	[Range(1,10)]
	public int smoothness;
	public string seed;
	public bool useRandomSeed = true;

	[Range(0, 100)]
	public int randomFillPercent;

	public float[,] mapWithValues;
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
		width = (int)(width / squareSize);
		height = (int)(height / squareSize);
		mapWithValues = new float[width, height];
		RandomFillMap(); // indexe bagli rastgelelikle doldur

		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
		}

		float[,] borderedMapWithValues = new float[width + (borderSize * 2), height + (borderSize * 2)];
		for (int x = 0; x < borderedMapWithValues.GetLength(0); x++)
		{
			for (int y = 0; y < borderedMapWithValues.GetLength(1); y++)
			{
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
					borderedMapWithValues[x, y] = mapWithValues[x - borderSize, y - borderSize];
                }
                else
                {
					borderedMapWithValues[x, y] = 1f;
                }
			}
		}
		
		

		DrawCicle(borderedMapWithValues, new Vector2Int(width / 2 * (int)squareSize, height / 2 * (int)squareSize), 0f, 5); // odanin ortasini bosalt
		
        // 0: up, 1: right, 2: down, 3: left
		int platformLength = (int) 5 / squareSize;
		int gateSpaceSize = (int)10 / squareSize;
		float gateDistanceToWall = 1.25f * squareSize;
		Vector3 roomBottomLeft = new Vector3(transform.position.x - (width/2) * squareSize, transform.position.y - (height/2) * squareSize, 0);
        if (gates[0]) // up
        {  

            DrawCicle(borderedMapWithValues, new Vector2Int(width / 2 , height - 1 ), 0, gateSpaceSize);
            DrawCicle(borderedMapWithValues, new Vector2Int(width / 2 , height - 3 ), 0, gateSpaceSize);

            for (int i = 0; i < platformLength; i++)
			{
                DrawCicle(borderedMapWithValues, new Vector2Int((width / 2) - ((int)(platformLength/2)) + i , height - gateSpaceSize ), 0.5f, 1);
            }
			GameObject g0 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3(width * squareSize / 2 ,  height * squareSize - (gateDistanceToWall - borderSize) -1, 0), Quaternion.Euler(0,0,90));
			g0.transform.parent = transform;

        }
        if (gates[1]) // right
        {
            DrawCicle(borderedMapWithValues, new Vector2Int(width - 1 , height / 2), 0, gateSpaceSize);
            DrawCicle(borderedMapWithValues, new Vector2Int(width - 3 , height / 2), 0, gateSpaceSize);
            for (int i = 0; i < platformLength; i++)
            {
                DrawCicle(borderedMapWithValues, new Vector2Int((width)-i , (height / 2) - gateSpaceSize), 0.5f, 1);
            }
            GameObject g1 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3( width * squareSize - (gateDistanceToWall - borderSize) -1,  height * squareSize / 2, 0), Quaternion.identity);
            g1.transform.parent = transform;

        }
        if (gates[2]) // down
        {

            DrawCicle(borderedMapWithValues, new Vector2Int(width / 2 , 0 ), 0, gateSpaceSize);
            DrawCicle(borderedMapWithValues, new Vector2Int(width / 2 , 2 ), 0, gateSpaceSize);
            for (int i = 0; i < platformLength; i++)
            {
                DrawCicle(borderedMapWithValues, new Vector2Int((width / 2) - ((int)(platformLength / 2)) + i , gateSpaceSize), 0.5f, 1);
            }
            GameObject g2 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3( width * squareSize / 2 , (gateDistanceToWall - borderSize ), 0), Quaternion.Euler(0, 0, -90));
            g2.transform.parent = transform;
        }
        if (gates[3]) // left
        {

            DrawCicle(borderedMapWithValues, new Vector2Int(0 , height / 2 ), 0, gateSpaceSize);
            DrawCicle(borderedMapWithValues, new Vector2Int(2 , height / 2 ), 0, gateSpaceSize);
            for (int i = 0;i < platformLength; i++)
			{
				DrawCicle(borderedMapWithValues, new Vector2Int(1+i , (height / 2) - gateSpaceSize), 0.5f, 1);
			}
            GameObject g3 = (GameObject)Instantiate(gatePrefab, roomBottomLeft + new Vector3((gateDistanceToWall - borderSize ) , height * squareSize / 2 , 0), Quaternion.Euler(0, 0, 180));
            g3.transform.parent = transform;
        }
		
        
				
		MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
		meshGenerator.GenerateMesh(borderedMapWithValues, squareSize); // MeshGenerator scriptinden fonksiyon. map gonderiliyor.
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
                    mapWithValues[x, y] = 1f;
				}
				else
				{
					// perlin noise doesnt use randomfillpercent, it can with a custom round function
					float val = Mathf.PerlinNoise((x / (float)width * smoothness) + pseudoRandom.Next(0, smoothness), (y / (float)height * smoothness) + pseudoRandom.Next(0, smoothness));// smoothness'i serpistirdim
					val = Mathf.Clamp(val, 0f, 1f);
					mapWithValues[x, y] = val;
					//map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0; // kalani rastgele 1 veya 0 ver random noise üret
				}
			}
		}
	}

	void SmoothMap() // rastgelelik duzgun hale getiriliyor
	{

		float[,] newMapWithValues = (float[,])mapWithValues.Clone();
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(x, y); // komsu duvar sayisi
				// TODO: mapWithValues surrounding wallarýn degerlerini topla ve ortalamasini al
				if (neighbourWallTiles > 4) // komsu duvar sayisi 4ten fazla ise duvar yap
				{
					newMapWithValues[x, y] += 0.5f;
				}
					
				else if (neighbourWallTiles < 4) // komsu duvar sayisi 4ten az ise bosluk yap
				{
					newMapWithValues[x, y] -= 0.5f;
				}
				newMapWithValues[x, y] = Mathf.Clamp(newMapWithValues[x, y], 0f, 1f);
			}
		}
		mapWithValues = newMapWithValues;
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
						wallCount += mapWithValues[neighbourX, neighbourY] > 0.5f? 1 : 0;
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

	

    private void DrawCicle(float[,] _map, Vector2Int coord, float value, int r)
    {
        if (r == 0) // brush boyutu en ufaksa direkt ciz
        {
            _map[coord.x, coord.y] = IsWithinGridBounds(_map, coord) ? value : _map[coord.x, coord.y];
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
  
    private bool IsWithinGridBounds(float[,] _map, Vector2Int cellCoords) // alanin disina tasmamasi icin
    {
        return cellCoords.x > 0 && cellCoords.x < _map.GetLength(0) - 1 && cellCoords.y > 0 && cellCoords.y < _map.GetLength(1) - 1;
    }
}