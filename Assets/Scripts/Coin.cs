using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] float y_range = 0.002f; //Y eksenindeki gidece�i miktar
    [SerializeField] float float_offset = 0f; // Coinlerin farkl� konumlarda ba�lamas� asenkron �al��mas�

    private void Start()
    {
        System.Random random = new System.Random(); //Rastgele Say� al�yor

        float_offset = random.Next(1,90); // 1 ile 90 aras� ki random yerde dola�s�n asenkron olsun.

    }
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + ((Mathf.Sin(Time.time + float_offset)) * y_range));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CollectibleManager>().AddCoin();
            gameObject.SetActive(false);
        }
    }
}