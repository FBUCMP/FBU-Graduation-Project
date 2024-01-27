using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class HPlayer : MonoBehaviour, IDataPersistance

{
    private bool isDead = false;
    public GameManagerScript gameManager; //Death Screen i�in gameManager �a��r�yoruz

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
        // burda bi�ey denedik olmad�
        //healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        if (healthBar == null)
        {
            this.AddComponent<HealthBar>();
            healthBar = GetComponent<HealthBar>();
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth); // ba�lang��ta texti maxhp/maxhp yazs�n diye
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("�l� m�: " + isDead);
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
            currentHealth = 0;
            healthBar.SetHealth(currentHealth, maxHealth);
            if (!isDead)
            {
                isDead = true;
                deathCount++;
                gameManager.gameOver();
                Debug.Log("Dead");
            }
        }
        healthBar.SetHealth(currentHealth, maxHealth);
    }

}