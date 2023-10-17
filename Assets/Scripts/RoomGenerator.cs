using UnityEngine;
using System.Collections;
using System;

public class RoomGenerator : MonoBehaviour
{

	public int width;
	public int height;
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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GenerateMap();
			Debug.Log(map.Length);
		}
	}

	void GenerateMap()
	{
		map = new int[width, height];
		RandomFillMap();
		
		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
		}
		MeshGenerator meshGenerator = GetComponent<MeshGenerator>();
		meshGenerator.GenerateMesh(map, 1); // MeshGenerator scriptinden fonksiyon. map gonderiliyor.
	}


	void RandomFillMap()
	{
		if (useRandomSeed)
		{
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) // dis sinirlari kesin duvar yap
				{
					map[x, y] = 1;
				}
				else
				{
					map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0; // rastgele 1 veya 0 ver random noise üret
				}
			}
		}
	}

	void SmoothMap()
	{

		int[,] newMap = (int[,])map.Clone(); 
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(x, y);

				if (neighbourWallTiles > 4)
					newMap[x, y] = 1;
					//map[x, y] = 1;
				else if (neighbourWallTiles < 4)
					newMap[x, y] = 0;
					//map[x, y] = 0;
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

	/*
	void OnDrawGizmos()
	{
		if (map != null)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
					Vector3 pos = new Vector3(-width/2 + x + .5f, -height/2 + y + .5f, 0);
					Gizmos.DrawCube(pos, Vector3.one);
				}
			}
		}
	}
	*/
}