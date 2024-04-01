using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    
    public bool isClosed = false;
    public Color openColor;
    public Color closedColor;
    public int direction; // 0: up, 1: right, 2: down, 3: left
    //public Transform exit;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    
    public delegate void GateAction(int dir);
    public static event GateAction OnGateCollide;
    void Start()
    {
        /*
        if (!exit)
        {
            exit = GetComponentInChildren<Transform>();
        }
        */
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
                //other.transform.position = exit.transform.position;
                OnGateCollide?.Invoke(direction);
            }
        }
    }

}
