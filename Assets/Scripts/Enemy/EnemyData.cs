using UnityEngine;

public enum EnemyType
{
    Dragon,
    Goblin
}
public class EnemyData : ScriptableObject
{
    public string Name;
    public float Health;
    public float Speed;
    public float Power;
    public GameObject Prefab;
    public EnemyType Type;
}

[CreateAssetMenu(fileName = "NewDragonData", menuName = "Enemy Data/Dragon", order = 52)]
public class DragonData : EnemyData
{
    public float FireBreathingPower;
}

[CreateAssetMenu(fileName = "NewGoblinData", menuName = "Enemy Data/Goblin", order = 53)]
public class GoblinData : EnemyData
{
    public float Stealth;
}
