using UnityEngine;


public class Enemy
{
    public float Health { get; set; }
    public float Speed { get; set; }
    public float Power { get; set; }
    public Vector2 Position { get; set; }

    public Enemy(float health, float speed, float power, Vector2 position)
    {
        Health = health;
        Speed = speed;
        Power = power;
        Position = position;
    }

    public void ModifyStats(float healthMultiplier, float speedMultiplier, float powerMultiplier)
    {
        Health *= healthMultiplier;
        Speed *= speedMultiplier;
        Power *= powerMultiplier;
    }
}