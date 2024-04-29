using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IKnockbackable
{
    public bool drawDebug = false;

    public float speed;
    public float power;


    [Header("Wall Avoidence")]
    public float maxSteeringForce = 4f;
    public float checkDistance = 0.9f; // avoid walls(solids) distance
    public Vector2Int minMaxWallCheckAngle = new Vector2Int(0, 360); // angle range for wall check - raycast
    public float reachDistance = 1.2f; // isTouchingWall distance
    public Vector2 reachOffset; // offset for reachDistance
    [Space(10)]
    public float visionDistance = 20; // how far the enemy can see
    public float memorySpan = 25; // how long the enemy remembers the player
    public LayerMask groundLayer; // layermask for the ground
    public LayerMask solidLayers; // ground wall + enemy layer

    public EnemyState enemyState = EnemyState.Idle; // current state of the enemy
    protected float currentVisionDistance; // current vision distance

    public float stillThreshold { get; set; } = 0.01f; // threshold for stillness

    public enum EnemyState
    {
        Idle,
        Follow,
        Attack
    }

    void Start()
    {
        currentVisionDistance = visionDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void GetKnockedBack(Vector3 force, float maxMoveTime)
    {
        
    }
}

