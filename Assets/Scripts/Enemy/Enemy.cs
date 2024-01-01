using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    public EnemyHealth health;
    public EnemyMovement movement;
    public EnemyPainResponse painResponse;

    private void Start()
    {
        health.OnTakeDamage += painResponse.HandlePain;
        health.OnDeath += Die;
    }

    private void Die(Vector3 Position)
    {
        movement.StopMoving();
        painResponse.HandleDeath();
    }
}