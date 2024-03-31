using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2Int spawnPosition = new Vector2Int(202, 714); // starting room position
    [Tooltip("Order in distance to Camera (close to far)")] public List<Sprite> sprites;
    public Color bgColor;
    public float size = 100;

    private List<GameObject> spriteHolders = new List<GameObject>();
    private Camera cam;
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
        Debug.Log("Parallax Executed: "+ roomCenter);
        spawnPosition = new Vector2Int((int)roomCenter.x, (int)roomCenter.y);
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        spriteHolders.Clear();
        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject spriteHolder = new GameObject("SpriteHolder"); // create another object sprite holder, for flat color
            spriteHolder.transform.position = transform.position;
            spriteHolder.transform.parent = transform;
            SpriteRenderer spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[i];
            spriteRenderer.sortingOrder = -i - 50;
            spriteRenderer.color = bgColor;
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
