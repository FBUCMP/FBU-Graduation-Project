using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyWalking : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    bool isFacingRight;
    [SerializeField] private float wait = 0.1f;
    Rigidbody2D rb;
    [SerializeField] GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    // coroutine kullanarak enemy'nin yönünü deðiþtirmesini saðlýyoruz
    private IEnumerator ChangeDirectionAfterDelay()
    {
        yield return new WaitForSeconds(wait);
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // obje oluþmamýþsa coroutine çalýþmasýn diye yoksa hata verir
        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeDirectionAfterDelay());
        }
    }
}
