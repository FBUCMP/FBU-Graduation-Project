using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    public void CreateEnemy(string name, float health, float speed, float power, Vector2 position, GameObject prefab)
    {
        Enemy newEnemy = new Enemy(name, health, speed, power, position, prefab);
        enemies.Add(newEnemy);
        DisplayEnemy(newEnemy);
    }

    public void ModifyEnemyStats(int enemyIndex, float healthMultiplier, float speedMultiplier, float powerMultiplier)
    {
        if (enemyIndex < 0 || enemyIndex >= enemies.Count)
        {
            Debug.LogError("Invalid enemy index!");
            return;
        }

        enemies[enemyIndex].ModifyStats(healthMultiplier, speedMultiplier, powerMultiplier);
    }

    private void DisplayEnemy(Enemy enemy)
    {
        GameObject enemyObject = Instantiate(enemy.Prefab, enemy.Position, Quaternion.identity);

        // enemyObject'un özelliklerini enemy'nin özelliklerine göre ayarla
        EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            Debug.Log("Enemy display");
            enemyController.Initialize(enemy);
        }
        else
        {
            Debug.LogWarning("EnemyController component not found on the instantiated enemy object!");
        }
    }
}
