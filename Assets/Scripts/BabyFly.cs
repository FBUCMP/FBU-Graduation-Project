using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFly : MonoBehaviour, IDamageable
{
    AudioSource audioSource;
    public AudioClip hissClip;

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
    private void Awake()
    {
        if (TryGetComponent(out AudioSource audio))
        {
            audioSource = audio;
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.1f;
        }

        _health = maxHealth;

        Invoke("PlayHiss", 0.5f);
        Invoke("DestroySelf", 5f);
    }
    void PlayHiss()
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(hissClip);
        }
    }
    void DestroySelf()
    {
        if (isActiveAndEnabled) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 hitPos = collision.ClosestPoint(transform.position);
            collision.gameObject.GetComponent<HPlayer>().TakeDamage(10,hitPos,1);
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
        }
    }
}
