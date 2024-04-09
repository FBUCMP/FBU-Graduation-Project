using System.Buffers.Text;
using UnityEngine;

public enum EnemyType
{
    Dragon,
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

[CreateAssetMenu(fileName = "NewHasakiFlyData", menuName = "Enemy Data/HasakiFly", order = 52)]
public class HasakiFlyData : EnemyData
{
    public float fireDamage;
    
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
