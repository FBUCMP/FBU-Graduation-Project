using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class HPlayer : MonoBehaviour
    
{

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
    public HealthBar healthBar;
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
        healthBar.SetMaxHealth(currentHealth,maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth); // ba�lang��ta texti maxhp/maxhp yazs�n diye
    } 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
         
        healthBar.SetHealth(currentHealth,maxHealth);
    }

}
