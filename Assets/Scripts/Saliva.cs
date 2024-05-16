using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saliva : MonoBehaviour, IDamageable
{
    public GameObject residue;
    public Transform spriteHolder; // assign in inspector

    private Rigidbody2D rb;

    [SerializeField]
    private int _health;
    [SerializeField]
    private int _maxHealth = 1;
    [SerializeField]
    private float _explosionEffect = 1f; // 0-1 value

    public int currentHealth { get => _health; set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; } // getter and setter
    public float explosionEffect { get => _explosionEffect; set => _explosionEffect = value; } // getter and setter

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag != "Player")
        {
            var contact = collision.GetContact(0);
            float angle = Mathf.Atan2(contact.normal.y, contact.normal.x) * Mathf.Rad2Deg;
            angle -= 90;
            GameObject newResidue = Instantiate(residue, contact.point, Quaternion.Euler(0, 0, angle));
            Destroy(gameObject);
        }
        
    }


    public void TakeDamage(int damage, Vector3 hitPos, float radius)
    {
        int damageTaken = Mathf.Clamp(damage, 0, currentHealth);

        currentHealth -= damageTaken;

        if (damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if (currentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
            Destroy(gameObject);
        }
    }
}
