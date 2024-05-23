using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Residue : MonoBehaviour
{
    float hitTimerSec = 0.5f;
    float hitTimer;
    private void Awake()
    {
        hitTimer = hitTimerSec;
        Invoke("DestroySelf",5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 hitPos = collision.ClosestPoint(transform.position);
            collision.gameObject.GetComponent<HPlayer>().TakeDamage(10, hitPos, 1);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                Vector3 hitPos = collision.ClosestPoint(transform.position);
                collision.gameObject.GetComponent<HPlayer>().TakeDamage(5, hitPos, 1);
                hitTimer = hitTimerSec;
            }
        }
    }
    void DestroySelf()
    {
        if (isActiveAndEnabled) Destroy(gameObject);
    }



}
