using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(FlashEffect))]
public class WalkingEnemyBehaviour : EnemyBehaviour
{
    private Rigidbody2D rb; // rigidbody of the enemy
    private Collider2D enemyCollider; // collider of the enemy
    private FlashEffect flashEffect; // flash effect turn on when enemy gets hit or about to explode
    private GameObject target; // the player

    private float gravityScale;
    private bool isAttacking = false;
    public Transform sprites;
    List<Vector2> waypoints = new List<Vector2>(); // enemy always follows the first ([0]) waypoint
    [SerializeField] private float pathUpdateSeconds = 1f;
    private Seeker seeker;
    private Path path;
    //float trackingAbility = 1f;
    float memoryTimer;
    private bool isJumping = false;
    private bool isGrounded;
    private bool isInAir = false;
    private bool isKnockedBack = false;
    private bool jumpEnabled = true;
    private bool isOnCoolDown = false;
    private int currentPathpoint = 0;
    private float nextWaypointDistance = 3f;

    private Vector2 normal;
    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        flashEffect = GetComponent<FlashEffect>();
    }
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        memoryTimer = memorySpan;
        gravityScale = rb.gravityScale;
        InvokeRepeating(nameof(UpdatePath), 0f, pathUpdateSeconds);
    }

   

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, 3f);
        if (!drawDebug) return;
        Color wpColor = Color.yellow;
        Gizmos.color = wpColor;
        Gizmos.DrawWireSphere(transform.position + (Vector3)reachOffset, reachDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position - transform.up * 0.3f, 1.25f);
        Gizmos.color = wpColor;

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
        CheckWall();
        WatchTarget();
        //Debug.DrawLine(rb.position, rb.position + rb.velocity, Color.blue); // draw the velocity vector
        HandleStates();
        //HandleWalls();
        //rb.gravityScale = isTouchingWall() ? 1 : gravityScale; // if enemy is close to a wall, reduce gravity
        if (isGrounded) 
        {
            isInAir = false;
            rb.gravityScale = 0f;
        }
        else
        {
            isInAir = true;
            rb.gravityScale = gravityScale;
        }
        
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(- Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        sprites.rotation = Quaternion.identity;
        Collider2D hit = Physics2D.OverlapCircle(transform.position - transform.up*0.3f , 1.25f, groundLayer);
        if (hit != null)
        {
            Vector2 to = hit.ClosestPoint(transform.position);
            RaycastHit2D[] results = new RaycastHit2D[1];
            Vector2 direction = to - (Vector2)transform.position;
            //Debug.Log("hitisnotnull");
            enemyCollider.Raycast(direction, results, direction.magnitude + 1f, groundLayer);
            if (results[0].collider != null)
            {
                //Debug.Log("resultsarenotnull");
                Debug.DrawLine(transform.position,  (Vector3)results[0].point, Color.red);
                normal = results[0].normal;
                sprites.rotation = Quaternion.FromToRotation(Vector3.up, normal); //Quaternion.Slerp(sprites.rotation, Quaternion.FromToRotation(Vector3.up, results[0].normal), Time.deltaTime * 20f); 
            }
            else
            {
                normal = Vector2.up;

            }
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
        int hitAmount = enemyCollider.Raycast(direction, results, currentVisionDistance);
        RaycastHit2D hit = results[0];
        
        if (hit.collider != null && hit.collider.CompareTag(target.tag)) // if the raycast hits the player
        {
            if (Vector3.Distance(rb.position, hit.collider.transform.position) < 2f)
            {
                ChangeState(EnemyState.Attack);
                return;
            }
            waypoints.Clear(); // clear all the waypoints
            waypoints.Add(hit.point); // add the player's position as the first waypoint
            memoryTimer = memorySpan; // reset the memory timer

            if (drawDebug) Debug.DrawLine(transform.position, transform.position + (target.transform.position - transform.position).normalized * currentVisionDistance, Color.green);

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
                if (Vector2.Distance(transform.position, waypoints[i]) < nextWaypointDistance) // if enemy close to a waypoint
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
        if (path == null)
        {
            //Debug.LogWarning("Path is null");
            return;
        }
        else
        {
            //Debug.Log("Path is not null");
        }
        if (currentPathpoint >= path.vectorPath.Count)
        {
            return;
        }
        currentVisionDistance = visionDistance*4;
        memoryTimer -= Time.fixedDeltaTime;
        if (waypoints.Count > 0) // moves here if there are waypoints
        {
            
            Vector2 desiredVelocity = ((Vector2)path.vectorPath[currentPathpoint] - rb.position).normalized * speed;

            
            Vector2 steeringForce = desiredVelocity - rb.velocity; // Calculate the steering force

            
            steeringForce = Vector2.ClampMagnitude(steeringForce, maxSteeringForce); // Limit the steering force to prevent excessive acceleration
            if (!isJumping && isGrounded && !isKnockedBack)
            {
                // -------------------------- MOVE --------------------------

                rb.velocity += steeringForce * Time.fixedDeltaTime; // Apply the steering force

                rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed); // Limit the velocity to the maximum speed

                // -------------------------- MOVE --------------------------

            }
            Debug.DrawLine(rb.position, (rb.position + rb.velocity), Color.blue);
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentPathpoint]);
            if (distance < nextWaypointDistance)
            {
                currentPathpoint++;
            }

            float dotProduct = Vector2.Dot(desiredVelocity.normalized, normal);

            
            float magnitudeSquared = normal.sqrMagnitude; // Calculate the magnitude squared of the normal vector

            
            Vector2 projection = (dotProduct / magnitudeSquared) * normal; // Calculate the projection of desiredVel onto normal

            Debug.DrawLine(rb.position, rb.position + (Vector2)projection, Color.green);
            //Debug.Log("Projection of desiredVel onto normal: " + projection);
            if (isGrounded && (projection + desiredVelocity.normalized).magnitude/2 > 0.6f) //desiredVelocity.normalized.y > 0.6f) // if the desired velocity is upwards jump
            {
                if (!isJumping && !isOnCoolDown)
                {
                    StartCoroutine(Jump(desiredVelocity.normalized)); // call jump here
                }
            }
                

        }
        else if (enemyState != EnemyState.Idle)
        {
            ChangeState(EnemyState.Idle);
        }

    }
    void Idle()
    {
        currentVisionDistance = visionDistance;
        if (Time.time % 2 == 0)
        {
            Vector2 direction = new Vector3(Random.Range(-1f,1f), Random.Range(0,2f));
            
            //rb.velocity = direction.normalized * speed;
            
            
        }
    }

    
    
    void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        seeker.enabled = false;
        path = null;
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
            if(drawDebug) Debug.DrawLine(transform.position, transform.position + (Vector3)rayDirection * checkDistance, enemyState == EnemyState.Follow ? Color.magenta:Color.white);

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
        //Debug.Log("Knocked" + force);
        Vector2 clamped = Vector2.ClampMagnitude(force, 1f);
        if (isKnockedBack) return;
        if (gameObject.activeSelf)
        {
            StartCoroutine(KnockBack(clamped, maxMoveTime));
        }
        
    }
    
    private IEnumerator KnockBack(Vector3 force, float maxMoveTime)
    {
        isKnockedBack = true;
        //rb.AddForce(force, ForceMode2D.Impulse);
        rb.velocity = new Vector2(force.x * gravityScale * 2.25f, force.y * gravityScale * 2.25f);
        yield return new WaitForSeconds(maxMoveTime);
        isKnockedBack = false;
    }
    private IEnumerator Jump(Vector2 dir)
    {
        isJumping = true;
        //Debug.Log("Jumping"+ dir);
        
        yield return new();
        // can play a sound for indication of jump
        yield return new WaitForSeconds(0.1f); 
        rb.velocity = new Vector2(dir.x * gravityScale*2.25f,dir.y * gravityScale*2.25f);
        //rb.AddForce(dir*100, ForceMode2D.Impulse);
        yield return new();
        
        while (true)
        {
            yield return new();
            if (isGrounded)
            {

                yield return new WaitForSeconds(0.1f);
                isJumping = false;

                StartCoroutine(JumpCoolDown());
                break;
            }
        }
    }
    

    void UpdatePath()
    {
        if (seeker.IsDone() && waypoints.Count>0)
        {
            seeker.StartPath(rb.position, waypoints[0], OnPathComplete); // calculate the path to waypoints[0] in some cases the position of the player in others first waypoint of the bloodhound smell
            
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p; // set the path to the calculated path
            currentPathpoint = 0;
        }
    }
    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }
    void CheckWall()
    {
        Collider2D hit = Physics2D.OverlapCircle(rb.position + reachOffset, reachDistance, groundLayer);
        isGrounded = hit != null;
    }
    void ChangeState(EnemyState newState)
    {
        //Debug.Log($"Changing state from {enemyState} to {newState}");
        enemyState = newState;
    }
}