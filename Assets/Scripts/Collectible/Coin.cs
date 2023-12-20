using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class Coin : MonoBehaviour
{
    

    void Start()
    {

    }


    void Update()
    {

        transform.position = new Vector2(transform.position.x, transform.position.y + ((Mathf.Sin(Time.time)) * 0.001f));
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