using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalking : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    bool isFacingRight;
    [SerializeField] private float wait = 5f;
    Rigidbody2D rb;
    [SerializeField] GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // Patrol düþmanýmýzýn yüzünün baktýðý yönü ve hýzý ayarlýyoruz
        if (IsFacingRight())
        {
            //Debug.Log("right");
            // move right
            rb.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            //Debug.Log("left");
            // move left
            rb.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    // enemy'nin yönünü deðiþtirmek için
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
        // Mathf.Epsilon deðeri 0'a çok yakýn bir deðerdir 0.0000001 gibi
    }

    // coroutine kullanarak enemy'nin yönünü deðiþtirmesini saðlýyoruz
    private IEnumerator ChangeDirectionAfterDelay()
    {
        yield return new WaitForSeconds(wait); // belirlenen bekleme süresince animasyonun gerçekleþme sýklýðý
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y); // yön deðiþimi
    }

    // enemyWalk prefabinde tanýmlanan colliderýn ilk noktasý bir baþka collidera çarptýðýnda yönünü deðiþtirmesi için
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // obje oluþmamýþsa coroutine çalýþmasýn diye yoksa hata verir
        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeDirectionAfterDelay()); // coroutine baþlangýcý
        }
    }
}