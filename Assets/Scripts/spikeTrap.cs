using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage;

    // Bu de�i�ken, s�rekli olarak hasar verme s�kl���n� kontrol eder
    [SerializeField] private float damageInterval = 1f;

    private float timeSinceLastDamage;

    private void Start()
    {
        timeSinceLastDamage = damageInterval;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Belirli bir s�re aral���nda hasar ver
            if (timeSinceLastDamage >= damageInterval)
            {
                collision.GetComponent<HPlayer>().TakeDamage(damage);
                timeSinceLastDamage = 0f; // Hasar verildikten sonra saya�� s�f�rla
            }
            else
            {
                timeSinceLastDamage += Time.deltaTime; // Saya�� artt�r
            }
        }
    }
}