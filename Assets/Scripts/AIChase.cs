using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{

    public GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float distanceBetween;
    Rigidbody2D rb; 

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distanceBetween = 10f;
        speed = 3f;

        // ba�lang��ta player objesini otomatik olarak yakalamas� i�in.
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Collider2D>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
       
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        // 1- kare baz�nda enemy bizi takip ederken soft bir d�n�� sa�las�n diye (1-2)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        
        // belli bir yak�nla�ma sa�land���nda harekete ge�mesi i�in
        // basit bir duyarl�l�k seviyesi
        // ba�lang�� seviyesini 10 yapt�m de�i�tirilebilir.
        // oyunun ana mapi tam olarak d�zenlendi�inde bu k�s�m da de�i�ecektir.
        // sadece tempScene'da denemek i�in yapt�m b�yle
        if(distance < distanceBetween)
        {
            rb.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            // 2-
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
