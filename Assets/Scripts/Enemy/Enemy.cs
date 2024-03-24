using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health;
    private float speed;
    private float power;
    public Vector2 position;
    public GameObject prefab;
    private string name;

    public Enemy(string name, float health, float speed, float power, Vector2 position, GameObject prefab)
    {
        this.health = health;
        this.speed = speed;
        this.power = power;
        this.position = position;
        this.name = name;
        this.prefab = prefab;
    }

    public void ModifyStats(float healthMultiplier, float speedMultiplier, float powerMultiplier)
    {
        health *= healthMultiplier;
        speed *= speedMultiplier;
        power *= powerMultiplier;
    }
}
