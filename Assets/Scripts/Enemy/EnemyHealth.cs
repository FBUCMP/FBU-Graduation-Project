using UnityEngine;

[DisallowMultipleComponent]
// stores enemy health and handles damage
// inherits from IDamageable
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _health;
    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private float _explosionEffect = 1f; // 0-1 value

    public int currentHealth { get => _health;  set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; } // getter and setter
    public float explosionEffect { get => _explosionEffect; set => _explosionEffect = value; } // getter and setter

    public event IDamageable.TakeDamageEvent OnTakeDamage; // spawnparticleondeath subscribes to those events
    public event IDamageable.DeathEvent OnDeath;

    private void OnEnable() // on enable incase of object pooling
    {
        _health = maxHealth;
    }

    public void TakeDamage(int damage, Vector3 hitPos, float r)
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