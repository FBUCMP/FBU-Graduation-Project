using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Residue : MonoBehaviour
{
    float hitTimer = 1;
    private void Awake()
    {
        Invoke("DestroySelf",5f);
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
                hitTimer = 1;
            }
        }
    }
        void DestroySelf()
    {
        Destroy(gameObject);
    }



}
