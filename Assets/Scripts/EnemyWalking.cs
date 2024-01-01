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

        // Patrol d��man�m�z�n y�z�n�n bakt��� y�n� ve h�z� ayarl�yoruz
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

    // enemy'nin y�n�n� de�i�tirmek i�in
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
        // Mathf.Epsilon de�eri 0'a �ok yak�n bir de�erdir 0.0000001 gibi
    }

    // coroutine kullanarak enemy'nin y�n�n� de�i�tirmesini sa�l�yoruz
    private IEnumerator ChangeDirectionAfterDelay()
    {
        yield return new WaitForSeconds(wait); // belirlenen bekleme s�resince animasyonun ger�ekle�me s�kl���
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y); // y�n de�i�imi
    }

    // enemyWalk prefabinde tan�mlanan collider�n ilk noktas� bir ba�ka collidera �arpt���nda y�n�n� de�i�tirmesi i�in
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // obje olu�mam��sa coroutine �al��mas�n diye yoksa hata verir
        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeDirectionAfterDelay()); // coroutine ba�lang�c�
        }
    }
}