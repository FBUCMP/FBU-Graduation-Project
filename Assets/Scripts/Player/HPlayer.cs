using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class HPlayer : MonoBehaviour, IDataPersistance

{
    [SerializeField] private bool isDead = false;
    //[SerializeField] private GameManagerScript gameManager; //Death Screen için gameManager çaðýrýyoruz



    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private int deathCount = 0;


    public HealthBar healthBar;

    public void LoadData(GameData gameData)
    {
        this.deathCount = gameData.deathCount;
    }

    public void SaveData(ref GameData gameData) 
    {
        gameData.deathCount = this.deathCount;
    }


    // Start is called before the first frame update
    void Start()
    {
        /*
        gameManager = GameManagerScript.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance is null!");
        }
        else
        {
            Debug.Log("GameManager instance is not null!");
        }
        */

        //healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        if (healthBar == null)
        {
            this.AddComponent<HealthBar>();
            healthBar = GetComponent<HealthBar>();
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth); // baþlangýçta texti maxhp/maxhp yazsýn diye
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ölü mü: " + isDead);
        if (Input.GetKeyDown(KeyCode.B))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Health is 0");
            currentHealth = 0;
            healthBar.SetHealth(currentHealth, maxHealth);
            if (!isDead)
            {
                isDead = true;
                deathCount++;
                GameManagerScript.Instance.gameOver(); 
                Debug.Log("Dead:" + deathCount);
            }
        }
        healthBar.SetHealth(currentHealth, maxHealth);
    }

}