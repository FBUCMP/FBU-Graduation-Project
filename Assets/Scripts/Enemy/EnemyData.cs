using System.Buffers.Text;
using UnityEngine;

public enum EnemyType
{
    Boss,
    Fly,
    Spider,
    Plant
}
[CreateAssetMenu(fileName = "NewGoblinData", menuName = "Enemy Data", order = 53)]
public class EnemyData : ScriptableObject
{
    public string name;
    public float health;
    public float speed;
    public float power;
    public GameObject prefab;
    public EnemyType type;

}

