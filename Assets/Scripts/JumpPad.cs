using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    private float bounceHeight;
    private PlayerMovementNew playerMovement; // referans de�i�keni
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementNew>();  // playerMovementi buluyoruz

        if (playerMovement != null)
        {
            bounceHeight = playerMovement.jumpHeight * 2f; // bounce de�erinin z�plama de�erinin 2 kat� olmas�n� istiyorum
           // Debug.Log("bounce: " + bounce);
        }
        else
        {
            Debug.Log("JumpPad.cs cannot find a PlayerMovement");
            bounceHeight = 4f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Mathf.Sqrt(-2f * bounceHeight * Physics2D.gravity.y), ForceMode2D.Impulse);
        }
    }
}
