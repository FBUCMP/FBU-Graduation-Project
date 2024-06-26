using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(FlashEffect))]
public class FlyingEnemyBehaviour : EnemyBehaviour
{
    private Rigidbody2D rb; // rigidbody of the enemy
    private Collider2D enemyCollider; // collider of the enemy
    private FlashEffect flashEffect; // flash effect turn on when enemy gets hit or about to explode

    private GameObject target; // the player

    private float gravityScale;


    List<Vector2> waypoints = new List<Vector2>(); // enemy always follows the first ([0]) waypoint
    private AIPath aiPath;
    private Seeker seeker;
    //float trackingAbility = 1f;
    float memoryTimer;
    private bool isAttacking = false;
    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        flashEffect = GetComponent<FlashEffect>();
    }
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        memoryTimer = memorySpan;
        gravityScale = rb.gravityScale;
    }

   

    private void OnDrawGizmos()
    {
        Color wpColor = Color.yellow;
        Gizmos.color = wpColor;
        Gizmos.DrawWireSphere(transform.position + (Vector3)reachOffset, reachDistance);
        if (waypoints.Count > 0 && drawDebug)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                wpColor.a = (float)i / waypoints.Count;
                Gizmos.DrawSphere(waypoints[i], 0.5f);
            }
            
        }
        Gizmos.color = Color.red;
        int j = minMaxWallCheckAngle.x; // min
        while (j < minMaxWallCheckAngle.y) // till reach max
        {
            float angle = j;
            Vector2 rayDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)rayDirection * checkDistance);
            j += 45;
        }
    }
    private void FixedUpdate()
    {
        WatchTarget();

        //Debug.DrawLine(rb.position, rb.position + rb.velocity, Color.blue); // draw the velocity vector
        HandleStates();
        HandleWalls();
        //rb.gravityScale = isTouchingWall() ? gravityScale / 10 : gravityScale; // if enemy is close to a wall, reduce gravity
        if (aiPath.velocity.x > 0)
        {
            transform.localScale = new Vector3(- Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (aiPath.velocity.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    void WatchTarget()
    {
        if (memoryTimer <= 0) // if enemy forgets
        {
            //Debug.Log($"{gameObject.name} forgot it's target ever existed...");
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
        int hitAmount = enemyCollider.Raycast(direction, results, visionDistance);
        RaycastHit2D hit = results[0];
        
        if (hit.collider != null && hit.collider.CompareTag(target.tag)) // if the raycast hits the player
        {
            if (Vector3.Distance(rb.position, hit.collider.transform.position) < 3f) // if the player is close enough to attack
            {
                ChangeState(EnemyState.Attack);
                return;
            }
            waypoints.Clear(); // clear all the waypoints
            waypoints.Add(hit.point); // add the player's position as the first waypoint
            memoryTimer = memorySpan; // reset the memory timer

            if (drawDebug) Debug.DrawLine(transform.position, transform.position + (target.transform.position - transform.position).normalized * visionDistance, Color.green);

        }
        else // hits something else (wall ...)
        {
            //Debug.DrawLine(transform.position, transform.position +(target.transform.position - transform.position).normalized * visionDistance, Color.red);
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
            
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (Vector2.Distance(transform.position, waypoints[i]) < 1.5f) // if enemy close to a waypoint
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
        memoryTimer -= Time.fixedDeltaTime;
        if (waypoints.Count > 0)
        {
            if (aiPath == null) // if A star is not used
            {
            
                Vector2 desiredVelocity = (waypoints[0] - rb.position).normalized * speed;

                // Calculate the steering force
                Vector2 steeringForce = desiredVelocity - rb.velocity;

                // Limit the steering force to prevent excessive acceleration
                steeringForce = Vector2.ClampMagnitude(steeringForce, maxSteeringForce);

                // Apply the steering force
                rb.velocity += steeringForce * Time.fixedDeltaTime;

                // Limit the velocity to the maximum speed
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
                

            }
            else
            {
                aiPath.destination = waypoints[0];
                //seeker.CancelCurrentPathRequest();
                //seeker.StartPath(rb.position, waypoints[0]);
                
            }

        }
        else if (enemyState != EnemyState.Idle)
        {
            ChangeState(EnemyState.Idle);
        }

    }
    void Idle()
    {
        if (Time.time % 2 == 0)
        {
            Vector3 direction = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),0);
            /*
            if (!aiPath.pathPending && (aiPath.reachedEndOfPath || !aiPath.hasPath))
            {
                aiPath.destination = transform.position + direction * 2;
                aiPath.SearchPath();
            }
            */
            aiPath.destination = transform.position + direction * 2; // might wanna fix this. shoudnt seperate idle move and follow move
            //seeker.CancelCurrentPathRequest();
            //seeker.StartPath(rb.position, waypoints[0]);
        }
    }
    void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        aiPath.enabled = false;
        rb.simulated = false;
        flashEffect.FlashBlink(Color.white, 1f, 3, 0.3f);
        Invoke("Explode", 1f);
    }

    void Explode()
    {
        float attackRadius = 2.5f;
        Collider2D[] hitObjects = new Collider2D[10];
        int hits = Physics2D.OverlapCircleNonAlloc(rb.position, attackRadius, hitObjects);
        for (int i = 0; i < hits; i++)
        {
            if (hitObjects[i].TryGetComponent(out IDamageable damagable))
            {

                damagable.TakeDamage(10 * (int)power, transform.position, attackRadius);
            }
        }
        if (TryGetComponent(out IDamageable selfDamagable))
        {
            selfDamagable.TakeDamage(selfDamagable.currentHealth, transform.position, 1f);
        }
    }
    void HandleWalls()
    {

        float maxForce = 0.6f;
        int angleBetweenRays = 45;
        Vector2 avoidance = Vector2.zero;
        int i = minMaxWallCheckAngle.x; // min
        while (i < minMaxWallCheckAngle.y) // till reach max
        {
            //float angle = i * 360 / rayCount; // if rayCount is 8: 0, 45, 90, 135, 180, 225, 270, 315
            float angle = i;
            Vector2 rayDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            if (drawDebug) Debug.DrawLine(transform.position, transform.position + (Vector3)rayDirection * checkDistance, enemyState == EnemyState.Follow ? Color.magenta:Color.white);

            RaycastHit2D[] hit = new RaycastHit2D[1];
            int hitCount = enemyCollider.Raycast(rayDirection, hit, checkDistance, solidLayers);
            if (hitCount > 0) // if enemy is close to a wall
            {
                if (drawDebug) Debug.DrawLine(transform.position, hit[0].point, Color.yellow);
                Vector2 newVec = (Vector2)transform.position - hit[0].point;
                newVec = newVec.normalized * (checkDistance - newVec.magnitude);
                avoidance += newVec;
            }
            i += angleBetweenRays;
        }
        avoidance = Vector2.ClampMagnitude(avoidance, maxForce);
        rb.velocity += avoidance;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
        
    }

    public override void GetKnockedBack(Vector3 force, float maxMoveTime)
    {
        aiPath.enabled = false;
        rb.AddForce(force);
        Invoke("EnableAIPath", maxMoveTime);
    }

    private void EnableAIPath()
    {
        aiPath.enabled = true;
    }

    bool isTouchingWall()
    {
        Collider2D hit = Physics2D.OverlapCircle(rb.position + reachOffset, reachDistance, groundLayer);
        return hit != null;
    }
    void ChangeState(EnemyState newState)
    {
        //Debug.Log($"Changing state from {enemyState} to {newState}");
        enemyState = newState;
    }
}