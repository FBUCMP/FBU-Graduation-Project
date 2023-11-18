using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class HPlayer : MonoBehaviour
    
{
    private bool isDead = false;
    public GameManagerScript gameManager; //Death Screen için gameManager çaðýrýyoruz

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        // burda biþey denedik olmadý
        //healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        if (healthBar == null)
        {
            this.AddComponent<HealthBar>();
            healthBar = GetComponent<HealthBar>();
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth,maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth); // baþlangýçta texti maxhp/maxhp yazsýn diye
    } 

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ölü mü: " + isDead);
        if(Input.GetKeyDown(KeyCode.B))
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
            healthBar.SetHealth(currentHealth,maxHealth);
            if (!isDead)
            {
                isDead = true;
                gameManager.gameOver();
                Debug.Log("Dead");
            }
        }
        healthBar.SetHealth(currentHealth, maxHealth);
    }

}
