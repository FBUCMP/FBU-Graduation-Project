using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHealth : MonoBehaviour, IDamageable
{
    private MeshGenerator meshGen;
    private RoomGenerator roomGen;
    private int _health;
    private int _maxHealth = 100;
    public int currentHealth { get => _health; private set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; private set => _maxHealth = value; } // getter and setter


    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage, Vector3 hitPos)
    {
        // get vertex with pos and lower its value
        float newdamage = damage / 10f;
        for (int i = 0; i < meshGen.squareGrid.controlNodes.GetLength(0); i++)
        {
            for (int j = 0; j < meshGen.squareGrid.controlNodes.GetLength(1); j++)
            {
                if (Vector3.Distance(meshGen.squareGrid.controlNodes[i, j].position, hitPos) < 0.5f)
                {
                    if (newdamage > meshGen.squareGrid.controlNodes[i, j].value)
                    {
                        newdamage = meshGen.squareGrid.controlNodes[i, j].value;
                    }
                    meshGen.squareGrid.controlNodes[i, j].value -= newdamage;
                    // recalculate and regenerate mesh
                }
            }
        }

        
    }

    // Start is called before the first frame update
    void Awake()
    {
        _health = _maxHealth;
    }
    private void Start()
    {
        meshGen = GetComponent<MeshGenerator>();
        roomGen = GetComponent<RoomGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
