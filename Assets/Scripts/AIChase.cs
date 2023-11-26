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

        // baþlangýçta player objesini otomatik olarak yakalamasý için.
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Collider2D>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
       
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        // 1- kare bazýnda enemy bizi takip ederken soft bir dönüþ saðlasýn diye (1-2)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        
        // belli bir yakýnlaþma saðlandýðýnda harekete geçmesi için
        // basit bir duyarlýlýk seviyesi
        // baþlangýç seviyesini 10 yaptým deðiþtirilebilir.
        // oyunun ana mapi tam olarak düzenlendiðinde bu kýsým da deðiþecektir.
        // sadece tempScene'da denemek için yaptým böyle
        if(distance < distanceBetween)
        {
            rb.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            // 2-
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
