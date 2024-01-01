using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class JumpPad : MonoBehaviour
{

    private float bounce;
    private PlayerMovement playerMovement; // referans deðiþkeni
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();  // playerMovementi buluyoruz

        if (playerMovement != null)
        {
            bounce = playerMovement.jumpingPower * 2f; // bounce deðerinin zýplama deðerinin 2 katý olmasýný istiyorum
           // Debug.Log("bounce: " + bounce);
        }
        else
        {
            Debug.Log("PlayerMovement component not found.");
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
