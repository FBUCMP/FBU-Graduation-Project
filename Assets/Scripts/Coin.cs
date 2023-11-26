using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class Coin : MonoBehaviour
{
    public Text scoreText; 
    private int score = 0;
    [SerializeField] float value = 0.002f;
    [SerializeField] float float_offset = 0f;

    void Start()
    {
        UpdateScoreText(); 
    }

    
    void Update()
    {
        
        transform.position = new Vector2(transform.position.x, transform.position.y + ((Mathf.Sin(Time.time + float_offset)) * value));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        
        score += 1;

        
        gameObject.SetActive(false);

       
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
       
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}