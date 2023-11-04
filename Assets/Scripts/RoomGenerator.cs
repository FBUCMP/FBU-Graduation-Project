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
	[Range(0,10)]
	public int smoothness;
	public string seed;
	public bool useRandomSeed = true;

	[Range(0, 100)]
	public int randomFillPercent;

	int[,] map; // bu script map arrayini 0-1 ler ile dolduruyor.
	void Start()
	{
		GenerateMap();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			GenerateMap();
			Debug.Log(map.Length);
		}
	}

	void GenerateMap() // bu bir room datasi olusturuyor
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
				
		MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
		meshGenerator.GenerateMesh(borderedMap, 1); // MeshGenerator scriptinden fonksiyon. map gonderiliyor.
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


}