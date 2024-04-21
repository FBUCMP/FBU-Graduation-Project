using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMap : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    public float[,] heatMap; // every index is 0 or 1 
    public float[,] heatMapSmooth;
    public int smoothness = 5;
    private RoomGenerator roomGenerator;
    void Awake()
    {
        roomGenerator = GetComponent<RoomGenerator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(!debugMode) return;
        if (heatMap != null)
        {
            for (int x = 0; x < heatMapSmooth.GetLength(0); x++)
            {
                for (int y = 0; y < heatMapSmooth.GetLength(1); y++)
                {
                    Gizmos.color = Color.Lerp(Color.yellow, Color.red, heatMapSmooth[x, y]);
                    // lower alpha
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 1f);
                    Gizmos.DrawCube(transform.position + new Vector3(x, y, 0) - new Vector3(roomGenerator.width/2 + 1f, roomGenerator.height/2 + 1f), Vector3.one);
                }
            }
        }
    }
    public void GenerateHeatMap(float[,] mapWithValues)
    { 
        // Create a 2D array of floats size of the room
        // Fill the array with values according to the room walls position
        heatMap = new float[mapWithValues.GetLength(0), mapWithValues.GetLength(1)];
        for (int x = 0; x < mapWithValues.GetLength(0); x++)
        {
            for (int y = 0; y < mapWithValues.GetLength(1); y++)
            {
                if (mapWithValues[x, y] >= 0.5)
                {
                    heatMap[x, y] = 1;
                }
                else
                {
                    mapWithValues[x, y] = 0;
                }
            }
        }
        heatMapSmooth = heatMap.Clone() as float[,];
        SmoothHeatMap(smoothness);
        
    }

    void SmoothHeatMap(int n)
    {
        // walls are 1, empty spaces are 0, if a node is close to a wall, it should have a higher value. it should be a gradient
        float[,] newHeatMap = heatMap.Clone() as float[,];
        for (int a = 0; a < n; a++)
        {
            float[,] innerHeatMap = newHeatMap.Clone() as float[,];
            for (int x = 0; x < newHeatMap.GetLength(0); x++)
            {
                for (int y = 0; y < newHeatMap.GetLength(1); y++)
                {
                    if (newHeatMap[x, y] == 1) // if 1 stays 1 if not, smooth it
                    {
                        innerHeatMap[x, y] = 1;
                    }
                    else
                    {

                        float sumOfNeighbors = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (x + i >= 0 && x + i < newHeatMap.GetLength(0) && y + j >= 0 && y + j < newHeatMap.GetLength(1)) // in bounds
                                {
                                    sumOfNeighbors += newHeatMap[x + i, y + j];
                                }
                            }
                        }

                        innerHeatMap[x, y] = sumOfNeighbors / 9; // play with this value
                    }
                }
            }

            newHeatMap = innerHeatMap;
        }
        heatMapSmooth = newHeatMap;
    }


    public void UpdateHeatMap(Bounds bound)
    {
        foreach (Vector3 pos in GetPositionsInBounds(bound))
        {
            Vector2Int heatMapPos = WorldToHeatMap(pos);
            //Debug.Log($"{gameObject.name} turned {heatMapPos} to 1"); 
            heatMap[heatMapPos.x, heatMapPos.y] = 1;
            SmoothHeatMap(smoothness);
        }
    }

    public bool IsInEmpty(Bounds bound, float threshold)
    {
        foreach (Vector3 pos in GetPositionsInBounds(bound))
        {
            Vector2Int heatMapPos = WorldToHeatMap(pos);
            if (heatMapSmooth[heatMapPos.x, heatMapPos.y] > threshold)
            {
                return false;
            }
        }
        return true;
    }

    Vector3 HeatMapToWorld(int x, int y)
    {
        return transform.position + new Vector3(x, y, 0) - new Vector3(roomGenerator.width / 2 + 1f, roomGenerator.height / 2 + 1f);
    }

    Vector2Int WorldToHeatMap(Vector3 pos)
    {
        // from pos convert where it corresponds in the heatmap
        Vector3 bottomLeft = transform.position - new Vector3(roomGenerator.width / 2 + 1f, roomGenerator.height / 2 + 1f);
        Vector3 relative = pos - bottomLeft;
        return new Vector2Int((int)relative.x, (int)relative.y);
    }

    List<Vector3> GetPositionsInBounds(Bounds bound)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int x = (int)bound.min.x; x < bound.max.x; x++)
        {
            for (int y = (int)bound.min.y; y < bound.max.y; y++)
            {
                positions.Add(new Vector3(x, y, 0));
            }
        }
        return positions;
    }
}
