using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    
    public bool isClosed = false;
    public Color openColor;
    public Color closedColor;
    public Transform exit;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    
    
    void Start()
    {
        if (!exit)
        {
            exit = GetComponentInChildren<Transform>();
        }
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider.size = spriteRenderer.transform.localScale;
        if (isClosed)
        {
            spriteRenderer.color = closedColor;       
        }
        else
        {
            spriteRenderer.color = openColor;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // if player colliders from one side , teleport to the other side of the gate
            if (!isClosed)
            {
                other.transform.position = exit.transform.position;
            }
        }
    }

}
