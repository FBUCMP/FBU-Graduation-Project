using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshGenerator : MonoBehaviour
{
	// kirmak icin control nodelari inactive yap. pos ile karsilastir

	[HideInInspector] public List<Vector3> vertices; // noktalarin pozisyonlari vector2 olmasi lazim tutorial 3d diye boyle degisebilir!
	List<int> triangles;
	Dictionary<int, List<Triangle>> triangleDict = new Dictionary<int, List<Triangle>>(); /* nokta, noktanin dahil oldugu ucgenler. amac dis hatlari bulmak*/
	List<List<int>> outlines = new List<List<int>>(); /* bir adet outline List<int>. tum outlinelar List<List<int>>. amac bunu bulmak*/
	HashSet<int> checkedVertices = new HashSet<int>(); /* tum vertexler checklenirken daha once checklenen denk gelmesin diye. hash contains fonksiyonu hizli.*/
    
    [HideInInspector]
    public float[,] mapWithValues;

   
    private void Update()
    {
	}

	public Vector2Int ClosestIndexToPos(Vector3 hitPos, int squareSize) // make this return array of closest indexes instead, can be empty
	{
		Vector2Int closest = Vector2Int.zero;
		for (int i = 0; i < mapWithValues.GetLength(0); i++)
		{
            for (int j = 0; j < mapWithValues.GetLength(1); j++)
			{
                if (mapWithValues[i,j] >= 0.5f && Vector3.Distance(GetPosFromIndex(new Vector2Int(i,j), squareSize), hitPos) < Vector3.Distance(GetPosFromIndex(closest, squareSize), hitPos))
				{
					closest = new Vector2Int(i, j);
                }
            }
        }
		return closest;
	}
	public Vector2Int[] ClosestIndexesToPos(Vector3 hitPos, int squareSize, int radius)
	{
		List<Vector2Int> indexes = new List<Vector2Int>();
		if (radius > 1)
		{
			for (int i = 0; i < mapWithValues.GetLength(0); i++)
			{
				for (int j = 0; j < mapWithValues.GetLength(1); j++)
				{
					if (mapWithValues[i, j] >= 0.5f && Vector3.Distance(GetPosFromIndex(new Vector2Int(i, j), squareSize), hitPos) < radius)
					{
						//Debug.Log(Vector3.Distance(GetPosFromIndex(new Vector2Int(i, j), squareSize), pos));
						indexes.Add(new Vector2Int(i, j));
					}
				}
			}
			indexes.Sort((a, b) => Vector3.Distance(GetPosFromIndex(a, squareSize), hitPos).CompareTo(Vector3.Distance(GetPosFromIndex(b, squareSize), hitPos)));

		}
		else // radius == 1
		{
			indexes.Add(ClosestIndexToPos(hitPos, squareSize));
		}
		
		return indexes.ToArray();
    }
	public Vector3 GetPosFromIndex(Vector2Int index, int squareSize) 
	{
        // if pos calculated like Vector3 pos = new Vector3(-mapWidth / 2 + (x * squareSize) + squareSize / 2, -mapHeight / 2 + (y * squareSize) + squareSize / 2, 0);
        // then reverse it to get the position from index
        Vector3 relativePos = new Vector3(-mapWithValues.GetLength(0) / 2 + (index.x * squareSize) + squareSize / 2, -mapWithValues.GetLength(1) / 2 + (index.y * squareSize) + squareSize / 2, 0);
        return gameObject.transform.position + relativePos;
        
    }
	public void GenerateMesh(float[,] map, int squareSize) // mesh and then colliders
    {
		GenerateMeshOnly(map, squareSize);

        GenerateColliders(); // fizik icin colliderlari olustur
    }
	public void GenerateMeshOnly(float[,] map, int squareSize)
	{
        this.mapWithValues = map;
        //Debug.Log(generateFromNothing);

        SquareGrid squareGrid = new SquareGrid(map, squareSize);
        triangleDict.Clear();
        outlines.Clear(); // generate mesh her callandiginda bu verileri resetle
        checkedVertices.Clear();
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]); // griddeki kareleri ucgenlestir mesh icin
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray(); // mesh icin gerekli veriler
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        int tileAmount = 10; // more then 1 should repeat the texture but it breaks
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].x) * tileAmount;
            float percentY = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].y) * tileAmount;
            uvs[i] = new Vector2(percentX, percentY);
        }
        mesh.uv = uvs;
    }
    void GenerateColliders()
    {
		EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>(); // halihazirda collider varsa
        for (int i = 0; i < currentColliders.Length; i++)
        {
			Destroy(currentColliders[i]); // onceki colliderlari temizle
        }

		CalculateMeshOutlines(); // outlinelari hesapla
        foreach (List<int> outline in outlines) // outlinelar icerisinden her bir outline kumesini dolas. outline noktalari tutuyor.
        {
			EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>(); // unitynin duz cizgi collideri
			Vector2[] edgePoints = new Vector2[outline.Count]; // edge collidera verilecek nokta konumlari

            for (int i = 0; i < outline.Count; i++)
            {
				edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].y); // 3D den 2D ye cast z yok
            }
			edgeCollider.points = edgePoints; // collider icin gereken noktalari ata
        }
    }


	void TriangulateSquare(Square square)
	/* A				  B
		 * O--------o--------O
		 * |				 |
		 * |				 |
		 * |				 |
		 * o				 o
		 * |				 |
		 * |				 |
		 * |				 |
		 * O--------o--------O 
		 * D				  C
	*/
{
	switch (square.configuration)
	{
		case 0:
			break;

		// 1 points:
		case 1:
			MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft); // D
			break;
		case 2:
			MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight); // C
			break;
		case 4:
			MeshFromPoints(square.topRight, square.centreRight, square.centreTop); // B
			break;
		case 8:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft); // A
			break;

		// 2 points:
		case 3:
			MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft); // C + D
			break;
		case 6:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom); // B + C
			break;
		case 9:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft); // A + D
			break;
		case 12:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft); // A + B
			break;
		case 5:
			MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft); // B + D
			break;
		case 10:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft); // A + C
			break;

		// 3 point:
		case 7:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft); // B + C + D
			break;
		case 11:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft); // A + C + D
			break;
		case 13:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft); // A + B + D
			break;
		case 14:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft); // A + B + C 
			break;

		// 4 point:
		case 15:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft); // A + B + C + D
				checkedVertices.Add(square.topLeft.vertexIndex); /* bu durumda noktalarin her biri meshin icinde kalacak yani disariyla temasi yok yani outline degil. o zaman bunlara outline mi diye hesaplamayý atla*/
				checkedVertices.Add(square.topRight.vertexIndex);
				checkedVertices.Add(square.bottomRight.vertexIndex);
				checkedVertices.Add(square.bottomLeft.vertexIndex);
			break;
	}

}

void MeshFromPoints(params Node[] points) /* params -> fonksiyon cagrilirken point0, point1, point2... seklinde parametre saglanacak params bu noktalari point arrayine atacak*/
	{
		AssignVertices(points);

		if (points.Length >= 3)
			CreateTriangle(points[0], points[1], points[2]);
		if (points.Length >= 4)
			CreateTriangle(points[0], points[2], points[3]);
		if (points.Length >= 5)
			CreateTriangle(points[0], points[3], points[4]);
		if (points.Length >= 6)
			CreateTriangle(points[0], points[4], points[5]);

	}

	void AssignVertices(Node[] points) /* tum maptaki noktalar burdan gececek ve vertices listesinde hepsinin pozisyonlari tutulacak */
	{
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i].vertexIndex == -1)
			{
				points[i].vertexIndex = vertices.Count;
				vertices.Add(points[i].position);
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c) // mesh olusturmak icin sadece noktalari vermek yetmiyor. noktalarin sirasi icin triangles arrayi de gerekiyor.
	{
		triangles.Add(a.vertexIndex); // mesh icin
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);

		Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex); // edge detect icin
		AddTriangleToDict(triangle.vertexIndexA, triangle);
		AddTriangleToDict(triangle.vertexIndexB, triangle);
		AddTriangleToDict(triangle.vertexIndexC, triangle);
	}

	void AddTriangleToDict(int vertexIndexKey, Triangle triangle)
    {
		if (triangleDict.ContainsKey(vertexIndexKey)) // nokta key olarak varsa ucgeni valuesuna ekle
        {
			triangleDict[vertexIndexKey].Add(triangle);
        }
        else // nokta yoksa noktayi key, yeni bir listeye triengle koyup value olarak ekle
        {
			List<Triangle> newTriangleList = new List<Triangle>();
			newTriangleList.Add(triangle);
			triangleDict.Add(vertexIndexKey,newTriangleList );
        }
    }
	void CalculateMeshOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex)) // daha once bu vertex checklenmediyse
            {
				int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex); // vertexIndex e outlinela bagli diger vertex
				if (newOutlineVertex != -1) // boyle bir vertex yoksa -1 varsa devam
				{
					checkedVertices.Add(vertexIndex); // vertexIndex checklendi

					List<int> newOutline = new List<int>(); 
					newOutline.Add(vertexIndex); // outline = [vertexIndex]
					outlines.Add(newOutline); // outlines List<List<int>> = [[vertexIndex]]
					FollowOutline(newOutlineVertex, outlines.Count - 1);
					outlines[outlines.Count - 1].Add(vertexIndex);
				}
			}
        }
    }

	void FollowOutline(int vertexIndex, int outlineIndex)
	{
		outlines[outlineIndex].Add(vertexIndex);
		checkedVertices.Add(vertexIndex);
		int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

		if (nextVertexIndex != -1)
		{
			FollowOutline(nextVertexIndex, outlineIndex);
		}
	}


	int GetConnectedOutlineVertex(int vertexIndex)
    {
        for (int i = 0; i < triangleDict[vertexIndex].Count; i++) // indexi verilen vertexin bulundugu ucgenleri loopla
        {
			Triangle triangle = triangleDict[vertexIndex][i];
            for (int j = 0; j < 3; j++) // her ucgenin kose vertexlerini loopla 3 adet var
            {
				int vertexB = triangle.vertices[j]; // sirayla her kose
                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB)) // ucgenin kosesinden biri zaten vertexIndex ise veya bu nokta checklendiyse onu atla
                {
					if (IsOutlineEdge(vertexIndex, vertexB)) // parametre ile ucgenin herhangi bir kosesi dis cizgi olusturuyor mu
					{
						return vertexB; // input parametresi ile dis cizgi olusturan vertex varsa returnle
					}
				}

                }
        }
		return -1;
    }
	bool IsOutlineEdge(int vertexA, int vertexB)
    {
		int sharedTriangleCount = 0; // counter
		for (int i = 0; i < triangleDict[vertexA].Count; i++)
        {
			if (triangleDict[vertexA][i].Contains(vertexB)) /* a noktasina sahip ucgenler icerisinde b ye de sahip olan varsa ortak yani shared ++. iki noktanin 1den fazla ortak ucgeni YOKSA
			                                                 * aralarindaki cizgi dis cizgidir = outline edgedir. */
            {
				sharedTriangleCount++;
				if (sharedTriangleCount > 1)
                {
					break;
                }
            }
        }
		return sharedTriangleCount == 1;
    }
	struct Triangle // ucgen. 3 kosesi ile olusturuluyor.
    {
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		public int[] vertices;
		public Triangle(int a, int b, int c)
        {
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;
			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;
		}
		public bool Contains(int vertexIndex) {

			return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
		}
	}

	public class SquareGrid // tile benzeri yapi olustur
	{
		public Square[,] squares; // tum karelerin tutuldugu 2d array
		public ControlNode[,] controlNodes;
        
        public SquareGrid(float[,] map, int squareSize) // room generatordan map datasi alacak. map datasi dolu yerler 1 bos yerler 0.
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + (x * squareSize) + squareSize / 2, -mapHeight / 2 + (y * squareSize) + squareSize / 2, 0); // map ortalansin diye -width/2 den basliyor
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] >= 0.5f, map[x, y],squareSize); // mapdeki index > 0.5se active degilse active degil. degisebilir!!!!!!!
			
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1]; // control nodelarin en sonundakilerin saginda kare olmayacak o yuzden -1 adet kare
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

	public class Square /* her bir kare. 8 noktasi var. koseler control node. kenarlarin ortalarindakiler node
	                     * koselerdeki control nodelarin aktifligine gore kenarlardaki nodelar arasi baglar kuruluyor ve mesh olusuyor
						*/
	{
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centreTop, centreRight, centreBottom, centreLeft;
		public int configuration;

		public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
		{
			topLeft = _topLeft;
			topRight = _topRight;
			bottomRight = _bottomRight;
			bottomLeft = _bottomLeft;

			centreTop = topLeft.right;
			centreRight = bottomRight.above;
			centreBottom = bottomLeft.right;
			centreLeft = bottomLeft.above;

			/* A				  B
			 * O--------o--------O
			 * |				 |
			 * |				 |
			 * |				 |
			 * o				 o
			 * |				 |
			 * |				 |
			 * |				 |
			 * O--------o--------O 
			 * D				  C
			 * 
			 * 0000
			 * A aktif ise 1000 yani 8 ekle
			 * B aktif ise 0100 yani 4 ekle
			 * C aktif ise 0010 yani 2 ekle
			 * D aktif ise 0001 yani 1 ekle
			 * 
			 * orn: A, B, C, D -> 1111
			 *		A ile C    -> 1010
			 *		
			 * yani configuration bir kare mesh in seklini tutuyor
			 * 
			 */
			if (topLeft.active)
				configuration += 8;
			if (topRight.active)
				configuration += 4;
			if (bottomRight.active)
				configuration += 2;
			if (bottomLeft.active)
				configuration += 1;
		}
	}

	public class Node
	{
		public Vector3 position;
		public int vertexIndex = -1; // mesh olusturmak icin

		public Node(Vector3 _pos)
		{
			position = _pos;
		}
	}

	public class ControlNode : Node
	{

		public bool active;
		public Node above, right; // kose control nodelari ustte ve yandaki normal nodelarý tutacak
		public float value;
		public ControlNode(Vector3 _pos, bool _active, float value, float squareSize) : base(_pos) // _pos'u inheritledigi classtan initle
		{
			active = _active;
			this.value = value;
			above = new Node(position + Vector3.up * squareSize * value); // x-y ekseni ise y icin vector.up
			right = new Node(position + Vector3.right * squareSize * value); // x ekseni vector.right
		}
        
    }
}