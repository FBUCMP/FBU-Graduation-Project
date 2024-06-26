using UnityEngine;

public interface IDamageable
{
    public int currentHealth { get; }
    public int maxHealth { get; }

    public float explosionEffect { get; } // 0-1 value

    public delegate void TakeDamageEvent(int damage);
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent(Vector3 position);
    public event DeathEvent OnDeath;

    public void TakeDamage(int damage, Vector3 hitPos, float radius);
}