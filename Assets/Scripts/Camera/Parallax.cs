using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2Int spawnPosition = new Vector2Int(202, 714); // starting room position
    [Tooltip("Order in distance to Camera (close to far)")] public List<Sprite> sprites;
    public Color bgColor;
    public Material litMaterial;
    public Material unlitMaterial;
    
    public float size = 100;

    private List<GameObject> spriteHolders = new List<GameObject>();
    private Camera cam;
    private List<int> fxLayers = new List<int>{ 1,3,6 }; // fx layers material is unlit to make it always visible
    void Start()
    {
        cam = Camera.main;

        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, transform.position.z);
        GateManager.OnTeleport += Execute;
        Execute(new Vector3(spawnPosition.x, spawnPosition.y, transform.position.z));
    }
    private void OnDisable()
    {
        GateManager.OnTeleport -= Execute;
    }
    void Execute(Vector3 roomCenter)
    {
        // when room is changed, gets executed
        spawnPosition = new Vector2Int((int)roomCenter.x, (int)roomCenter.y); // update spawn position
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        spriteHolders.Clear();

        // set the background sprites
        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject spriteHolder = new GameObject("SpriteHolder"); // create another object sprite holder, for flat color
            spriteHolder.transform.position = transform.position;
            spriteHolder.transform.parent = transform;
            SpriteRenderer spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[i];
            spriteRenderer.sortingOrder = -i - 50;
            spriteRenderer.color = bgColor;
            // if fx layers has i
            if (fxLayers.Contains(i))
            {
                spriteRenderer.material = unlitMaterial;
                // lower alpha
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
            else
            {
                spriteRenderer.material = litMaterial;
            }
            spriteHolder.transform.localScale = new Vector2(size, size);
            spriteHolders.Add(spriteHolder);
            /*
            Vector2[] directions = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
            foreach (Vector2 dir in directions)
            {
                GameObject spriteHolderChild = new GameObject("SpriteHolderChild");
                spriteHolderChild.transform.parent = spriteHolder.transform;
                spriteHolderChild.transform.localScale = new Vector2(1, 1);
                SpriteRenderer childRenderer = spriteHolderChild.AddComponent<SpriteRenderer>();
                childRenderer.sprite = sprites[i];
                childRenderer.sortingOrder = -i - 50;
                childRenderer.color = bgColor;

                spriteHolderChild.transform.position = new Vector3(spriteHolder.transform.position.x + (dir.x * size), spriteHolder.transform.position.y + (dir.y * size / 2), spriteHolder.transform.position.z);

            }
            */


        }
    }
    void FixedUpdate()
    {
        
        // move the background with the camera this script is attached to a child of main camera
        // checks the distance between camera and the spawn position and moves the background accordingly to the layer order
        // when room is changed, spawn position is updated to the new room center
        for(int i = 0; i < spriteHolders.Count; i++)
        {
            Vector2 camDist = (Vector2)cam.transform.position - spawnPosition;
            float parallaxEffect = (float)i / spriteHolders.Count;
            Vector2 temp = camDist * (1 - parallaxEffect);
            Vector2 dist = camDist * parallaxEffect;
            spriteHolders[i].transform.position = new Vector3(spawnPosition.x + dist.x, spawnPosition.y + dist.y, spriteHolders[i].transform.position.z);
            /*
            if (temp.x > size)
            {
                spawnPosition += new Vector2Int((int)size, 0);
                Debug.Log(temp);
            }
            else if (temp.x < -size)
            {
                spawnPosition -= new Vector2Int((int)size, 0);
                Debug.Log(temp);
            }
            if (temp.y > size/4)
            {
                spawnPosition += new Vector2Int(0, (int)size);
                Debug.Log(temp);
            }
            else if (temp.y < -size/4)
            {
                spawnPosition -= new Vector2Int(0, (int)size);
                Debug.Log(temp);
            }
            */
        }

        
    }
}
