using UnityEngine;

public class Enemy
{
    public string Name { get; set; }
    public float Health { get; set; }
    public float Speed { get; set; }
    public float Power { get; set; }
    public Vector2 Position { get; set; }
    public GameObject Prefab { get; set; }

    public Enemy(string name, float health, float speed, float power, Vector2 position, GameObject prefab)
    {
        Name = name;
        Health = health;
        Speed = speed;
        Power = power;
        Position = position;
        Prefab = prefab;
    }

    public void ModifyStats(float healthMultiplier, float speedMultiplier, float powerMultiplier)
    {
        Health *= healthMultiplier;
        Speed *= speedMultiplier;
        Power *= powerMultiplier;
    }
}
