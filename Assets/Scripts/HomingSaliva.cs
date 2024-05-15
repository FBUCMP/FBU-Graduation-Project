using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSaliva : MonoBehaviour, IDamageable
{
    public ParticleSystem homingDestroyedParticle;
    public float speed = 5f;
    private Rigidbody2D rb;
    private Rigidbody2D playerRB;

    [SerializeField]
    private int _health;
    [SerializeField]
    private int _maxHealth = 100;
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
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // keep rotating around the z axis
        transform.Rotate(0, 0, rb.velocity.magnitude);
        // move towards the player
        Vector2 direction = (Vector2)playerRB.transform.position - rb.position;
        rb.velocity = direction.normalized * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 hitPos = collision.GetContact(0).point;
            collision.gameObject.GetComponent<HPlayer>().TakeDamage(10, hitPos, 1);

            Instantiate(homingDestroyedParticle, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        else
        {
            Instantiate(homingDestroyedParticle, transform.position, Quaternion.identity);

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
