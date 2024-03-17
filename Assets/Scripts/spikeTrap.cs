using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage;

    // Bu deðiþken, sürekli olarak hasar verme sýklýðýný kontrol eder
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
            // Belirli bir süre aralýðýnda hasar ver
            if (timeSinceLastDamage >= damageInterval)
            {
                collision.GetComponent<HPlayer>().TakeDamage(damage);
                timeSinceLastDamage = 0f; // Hasar verildikten sonra sayaçý sýfýrla
            }
            else
            {
                timeSinceLastDamage += Time.deltaTime; // Sayaçý arttýr
            }
        }
    }
}