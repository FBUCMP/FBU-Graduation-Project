using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Pathfinding;

public class RoomHealth : MonoBehaviour, IDamageable
{
    // attached to the room object to make it breakable

    private MeshGenerator meshGen;
    private RoomGenerator roomGen;
    private int _health;
    private int _maxHealth = 100;

    private float _explosionEffect = 1f; // 0-1 value
    private float resistance = 20f; // how much ressistant the wall is to the damage
    public int currentHealth { get => _health; private set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; private set => _maxHealth = value; } // getter and setter
    public float explosionEffect { get => _explosionEffect; set => _explosionEffect = value; } // getter and setter

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage, Vector3 hitPos, float radius)
    {
        //Debug.Log("RoomHealth TakeDamage");
        int r = Mathf.CeilToInt(radius);
        float newDamage = damage / resistance;
        Vector2Int[] indexes = meshGen.ClosestIndexesToPos(hitPos, roomGen.squareSize, r); // closes map index to the hit position

        Bounds bounds = new Bounds(hitPos, new Vector3(2*r, 2*r, 0)); // bounds holds the area that will be updated because of breaking the wall
        AstarPath.active.UpdateGraphs(bounds); // pass the bounds to the A star pathfinding graph to update just the area that will be updated

        if (indexes.Length > 0)
        {
            if (r <= 1)
            {
                float damageApplied = Mathf.Min(newDamage, meshGen.mapWithValues[indexes[0].x, indexes[0].y]); // fix index out of range
                ApplyDamage(damageApplied, indexes[0]);
                meshGen.GenerateMesh(meshGen.mapWithValues, roomGen.squareSize);
            }
            else
            {
                Debug.DrawLine(hitPos, meshGen.GetPosFromIndex(indexes[0], roomGen.squareSize), Color.red, 1f);
                for (int i = 0; i < indexes.Length; i++)
                {
                    float distance = Vector2.Distance(meshGen.GetPosFromIndex(indexes[i], roomGen.squareSize), hitPos);
                    float damageApplied = Mathf.Min(newDamage * (1 - distance / r), meshGen.mapWithValues[indexes[i].x, indexes[i].y]);
                    ApplyDamage(damageApplied, indexes[i]);
                }
                meshGen.GenerateMesh(meshGen.mapWithValues, roomGen.squareSize);
            }
        }
        else
        { 
            Debug.Log("No indexes found");
        }

    }

    void ApplyDamage(float damage, Vector2Int index) // actually apply the damage to the wall
    {
        if (IsInBounds(index.x, index.y))
        {
            float damageApplied = Mathf.Min(damage, meshGen.mapWithValues[index.x, index.y]);

            meshGen.mapWithValues[index.x, index.y] -= damageApplied; // damage is applied here
            

            // rest is mendatory for the interface
            if (damageApplied != 0)
            {
                OnTakeDamage?.Invoke((int)damageApplied);
            }

            if (currentHealth == 0 && damageApplied != 0)
            {
                OnDeath?.Invoke(transform.position);
            }
        }
    }

    void Awake()
    {
        _health = _maxHealth;
    }
    private void Start()
    {
        meshGen = GetComponent<MeshGenerator>();
        roomGen = GetComponent<RoomGenerator>();
        if (meshGen == null)
        {
            Debug.LogError("MeshGenerator component not found.");
        }
    }

    bool IsInBounds(int x, int y)
    {
        return x > roomGen.borderSize && x < (meshGen.mapWithValues.GetLength(0) - roomGen.borderSize) 
            && y > roomGen.borderSize && y < (meshGen.mapWithValues.GetLength(1) - roomGen.borderSize);
    }
}
