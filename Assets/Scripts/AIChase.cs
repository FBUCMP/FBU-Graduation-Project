using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{

    public GameObject player;
    public float speed;
    public float distanceBetween = 4;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        // baþlangýç seviyesini 4 yaptým deðiþtirilebilir.
        // oyunun ana mapi tam olarak düzenlendiðinde bu kýsým da deðiþecektir.
        // sadece tempScene'da denemek için yaptým böyle
        if(distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            // 2-
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
