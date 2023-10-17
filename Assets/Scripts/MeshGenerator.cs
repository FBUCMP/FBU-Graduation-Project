using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshGenerator : MonoBehaviour
{
	// kirmak icin control nodelari inactive yap. pos ile karsilastir

	public SquareGrid squareGrid;

	List<Vector3> vertices; // noktalarin pozisyonlari
	List<int> triangles;

	public void GenerateMesh(int[,] map, float squareSize)
	{
		squareGrid = new SquareGrid(map, squareSize);

		vertices = new List<Vector3>();
		triangles = new List<int>();

		for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
		{
			for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
			{
				TriangulateSquare(squareGrid.squares[x, y]);
			}
		}

		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

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
			MeshFromPoints(square.centreBottom, square.bottomLeft, square.centreLeft); // D
			break;
		case 2:
			MeshFromPoints(square.centreRight, square.bottomRight, square.centreBottom); // C
			break;
		case 4:
			MeshFromPoints(square.centreTop, square.topRight, square.centreRight); // B
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

	void CreateTriangle(Node a, Node b, Node c)
	{
		triangles.Add(a.vertexIndex);
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);
	}



	public class SquareGrid // tile benzeri yapi olustur
	{
		public Square[,] squares; // tum karelerin tutuldugu 2d array

		public SquareGrid(int[,] map, float squareSize) // room generatordan map datasi alacak. map datasi dolu yerler 1 bos yerler 0.
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
					controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize); // mapdeki index 1se active degilse active degil. degisebilir!!!!!!!
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

		public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos) // _pos'u inheritledigi classtan initle
		{
			active = _active;
			above = new Node(position + Vector3.up * squareSize / 2f); // x-y ekseni ise y icin vector.up
			right = new Node(position + Vector3.right * squareSize / 2f); // x ekseni vector.right
		}

	}
}