using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    public float memorySpan = 15f; // how long the enemy remembers the player
    public LayerMask groundLayer; // layermask for the ground
    private Rigidbody2D rb; // rigidbody of the enemy
    private Collider2D enemyCollider; // collider of the enemy

    private GameObject target; // the player

    List<Vector2> waypoints = new List<Vector2>(); // enemy always follows the first ([0]) waypoint
    float trackingAbility = 1f;
    float memoryTimer;
    public EnemyState enemyState = EnemyState.Idle; // current state of the enemy
    public enum EnemyState
    {
        Idle,
        Follow,
        Attack
    }
    void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        memoryTimer = memorySpan;
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Color wpColor = Color.yellow;
        if (waypoints.Count > 0)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                wpColor.a = (float)i / waypoints.Count;
                Gizmos.color = wpColor;
                Gizmos.DrawSphere(waypoints[i], 0.5f);
            }
            
        }
    }
    private void FixedUpdate()
    {
        WatchTarget();

        Debug.DrawLine(rb.position, rb.position + rb.velocity, Color.blue); // draw the velocity vector
        HandleStates();
        HandleWalls();
    }
    void WatchTarget()
    {
        memoryTimer -= Time.deltaTime;
        if (memoryTimer <= 0) // if enemy forgets
        {
            Debug.Log($"{gameObject.name} forgot it's target ever existed...");
            memoryTimer = memorySpan;
            waypoints.Clear(); // clear all the waypoints and go back to idle state
            if (enemyState != EnemyState.Idle)
            {
                ChangeState(EnemyState.Idle);
            }
        }
        /* using collider raycast bc doesnt include own collider
         * collider.Raycast (direction) => RaycastHit2D[]
         */

        Vector2 direction = target.transform.position - transform.position; // direction from enemy to the target
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitAmount = enemyCollider.Raycast(direction, results);
        RaycastHit2D hit = results[0];
        
        if (hit.collider != null && hit.collider.CompareTag(target.tag)) // if the raycast hits the player
        {
            waypoints.Clear(); // clear all the waypoints
            waypoints.Add(hit.point); // add the player's position as the first waypoint

            Debug.DrawLine(transform.position, target.transform.position, Color.green);

        }
        else // hits something else (wall ...)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        }


        if (waypoints.Count > 0 && Vector2.Distance(waypoints[waypoints.Count-1], target.transform.position) > 2f) // if the player moves away from the last waypoint
        {
            waypoints.Add(target.transform.position); // add the player's current position as a new waypoint to the end of the list
        }

        if (waypoints.Count == 0)
        {
            if (enemyState != EnemyState.Idle) // if no waypoints, go to idle state
            {
                ChangeState(EnemyState.Idle);
            }
        }
        else // if there are waypoints
        {
            if (enemyState != EnemyState.Follow) 
            { 
                ChangeState(EnemyState.Follow); // if there are waypoints, go to follow state
            }
            /*
            if (Vector2.Distance(transform.position, waypoints[0]) < 2f) // if enemy close to the waypoint
            {
                waypoints.RemoveAt(0); // remove that waypoint
            }
            */
            
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (Vector2.Distance(transform.position, waypoints[i]) < 2f) // if enemy close to a waypoint
                {
                    if (i == waypoints.Count-1) // if the last waypoint
                    {
                        waypoints.RemoveAt(i); // remove that waypoint
                    }
                    else
                    {
                        waypoints.RemoveRange(0, i+1); // remove that waypoint and all the previous ones
                    
                    }
                }
            }

            
        }

    }
    void HandleStates()
    {
        switch (enemyState)
        {
            case EnemyState.Follow:
                FollowPlayer();
                break;
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
        

    }
    void FollowPlayer()
    {
        /*
        if (waypoints.Count > 0 && Time.time % trackingAbility == 0)
        {
            Debug.Log("Removing waypoint");
            waypoints.RemoveAt(0);
        }
        */
        if (waypoints.Count > 0)
        {
            Vector2 direction = waypoints[0] - rb.position;
            rb.velocity = direction.normalized * speed;
            
        }
        else if (enemyState != EnemyState.Idle)
        {
            enemyState = EnemyState.Idle;
        }

    }
    void Idle()
    {
        if (Time.time % 2 == 0)
        {
            Vector2 direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
            rb.velocity = direction.normalized * speed / 3;
        }
    }
    void Attack()
    {
        Debug.Log(gameObject.name + " Attacking");
    }
    void HandleWalls()
    {
        float checkDistance = 1f;
        float power = 1f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkDistance, groundLayer);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * checkDistance, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + Vector3.left * checkDistance, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + Vector3.up * checkDistance, Color.magenta);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * checkDistance, Color.magenta);
        
        Vector2 distToWall;
        foreach (Collider2D collider in colliders)
        {
            distToWall = collider.ClosestPoint(transform.position);
            //Debug.Log((transform.position - (Vector3)distToWall) * power);
            Debug.DrawLine(transform.position, distToWall, Color.yellow);
            rb.velocity = 
            rb.velocity += (rb.position - distToWall) * power;
            //rb.AddForce((transform.position - (Vector3)distToWall) * power , ForceMode2D.Impulse);
        }
        
    }
    void ChangeState(EnemyState newState)
    {
        Debug.Log($"Changing state from {enemyState} to {newState}");
        enemyState = newState;
    }
}