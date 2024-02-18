using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBackground : MonoBehaviour
{
    public Color bgColor;
    public Sprite sprite;
    public Material material;
    private RoomGenerator roomGenerator;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        roomGenerator = GetComponent<RoomGenerator>();
        GameObject spriteHolder = new GameObject("SpriteHolder");
        spriteHolder.transform.position = transform.position;
        spriteHolder.transform.parent = transform;
        spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.material = material;
        spriteRenderer.sortingOrder = -1;
        spriteHolder.transform.localScale = new Vector3(roomGenerator.width, roomGenerator.height, 1);
        spriteRenderer.color = bgColor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
