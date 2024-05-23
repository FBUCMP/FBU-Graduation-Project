using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class HPlayer : MonoBehaviour, IDataPersistance, IDamageable

{
    [SerializeField] private bool isDead = false;
    //[SerializeField] private GameManagerScript gameManager; //Death Screen için gameManager çaðýrýyoruz

    [SerializeField]
    private int _health;
    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private float _explosionEffect = 0.01f; // 0-1 value
    public int currentHealth { get => _health; set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; } // getter and setter
    public float explosionEffect { get => _explosionEffect; set => _explosionEffect = value; } // getter and setter

    

    private int deathCount = 0;


    public HealthBar healthBar;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    

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
            TakeDamage(20, transform.position,1f);
        }
    }

    public void TakeDamage(int damage, Vector3 hitPos, float radius) // radius 1f
    {
        damage = Mathf.Clamp(damage, 0, currentHealth);
        currentHealth -= damage;
        if (damage != 0)
        {
            OnTakeDamage?.Invoke(damage);
        }
        if (currentHealth <= 0)
        {
            Debug.Log("Health is 0");
            currentHealth = 0;
            healthBar.SetHealth(currentHealth, maxHealth);
            if (!isDead)
            {
                isDead = true;
                deathCount++;
                OnDeath?.Invoke(transform.position);
                GameManagerScript.Instance.gameOver(); 
                Debug.Log("Dead:" + deathCount);
            }
        }
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        if (!isDead)
        {
            maxHealth += amount;
           
            healthBar.SetMaxHealth(currentHealth, maxHealth);
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth, maxHealth);
    }
    
}