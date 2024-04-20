using System.Buffers.Text;
using UnityEngine;

public enum EnemyType
{
    Boss,
    Goblin,
    Spider
}
public class EnemyData : ScriptableObject
{
    public string name;
    public float health;
    public float speed;
    public float power;
    public GameObject prefab;
    public EnemyType type;

}

[CreateAssetMenu(fileName = "NewGoblinData", menuName = "Enemy Data/Goblin", order = 53)]
public class GoblinData : EnemyData
{
    public float iceDamage;
}
[CreateAssetMenu(fileName = "NewSpiderData", menuName = "Enemy Data/Spider", order = 53)]
public class SpiderData : EnemyData
{
    public float webDamage;
}
[CreateAssetMenu(fileName = "NewBossData", menuName = "Enemy Data/Boss", order = 53)]
public class Boss : EnemyData
{
    public float acidDamage;
}
