using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    private float bounce;
    private PlayerMovementNew playerMovement; // referans deðiþkeni
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementNew>();  // playerMovementi buluyoruz

        if (playerMovement != null)
        {
            bounce = playerMovement.jumpingPower * 2f; // bounce deðerinin zýplama deðerinin 2 katý olmasýný istiyorum
           // Debug.Log("bounce: " + bounce);
        }
        else if (FindObjectOfType<PlayerMovementNew>())
        {
            bounce = FindObjectOfType<PlayerMovementNew>().jumpingPower*2f;
            
        }
        else
        {
            Debug.Log("JumpPad.cs cannot find a PlayerMovement");
            bounce = 16f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
}
