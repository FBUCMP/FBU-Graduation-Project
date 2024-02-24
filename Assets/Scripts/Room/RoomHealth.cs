using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class RoomHealth : MonoBehaviour, IDamageable
{
    private MeshGenerator meshGen;
    private RoomGenerator roomGen;
    private int _health;
    private int _maxHealth = 100;
    private float resistance = 20f;
    public int currentHealth { get => _health; private set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; private set => _maxHealth = value; } // getter and setter


    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(int damage, Vector3 hitPos, float radius)
    {
        int r = Mathf.CeilToInt(radius);
        float newDamage = damage / resistance;
        Profiler.BeginSample("ClosestIndexesToPos");
        Vector2Int[] indexes = meshGen.ClosestIndexesToPos(hitPos, roomGen.squareSize, r); // closes map index to the hit position
        Profiler.EndSample();

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
                /*
                for (int x = index.x - r; x < index.x + r; x++)
                {
                    for (int y = index.y - r; y < index.y + r; y++)
                    {
                        //float distance = Vector2.Distance(meshGen.GetPosFromIndex(new Vector2Int(x,y), roomGen.squareSize), meshGen.GetPosFromIndex(index, roomGen.squareSize));
                        float distance = MathF.Sqrt(MathF.Pow(x - index.x, 2) + MathF.Pow(y - index.y, 2));
                        float damageApplied = Mathf.Min(newDamage * (1 - distance / r), meshGen.mapWithValues[x, y]);
                        ApplyDamage(damageApplied, new Vector2Int(x, y), r);
                    }
                }
                */
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

    void ApplyDamage(float damage, Vector2Int index)
    {
        if (IsInBounds(index.x, index.y))
        {
            float damageApplied = Mathf.Min(damage, meshGen.mapWithValues[index.x, index.y]);

            meshGen.mapWithValues[index.x, index.y] -= damageApplied;
            

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
        else
        {
            Debug.Log($"Wall you try to break ({index.x}, {index.y}) is out of bounds");
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

    // Update is called once per frame
    bool IsInBounds(int x, int y) // fix this
    {
        return x > roomGen.borderSize && x < (meshGen.mapWithValues.GetLength(0) - roomGen.borderSize) 
            && y > roomGen.borderSize && y < (meshGen.mapWithValues.GetLength(1) - roomGen.borderSize);
    }
}
