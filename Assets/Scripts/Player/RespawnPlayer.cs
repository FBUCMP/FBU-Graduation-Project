using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject player;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = respawnPoint.transform.position;
        }
    }

}
