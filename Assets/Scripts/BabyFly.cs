using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFly : MonoBehaviour
{
    private void Awake()
    {
        Invoke("DestroySelf", 5f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 hitPos = collision.ClosestPoint(transform.position);
            collision.gameObject.GetComponent<HPlayer>().TakeDamage(10,hitPos,1);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
          
        }


    }

}
