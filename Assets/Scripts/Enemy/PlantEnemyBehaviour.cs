using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlantEnemyBehaviour : EnemyBehaviour
{
    public GameObject homingSalivaPrefab;
    public Transform attackSpawnPos;
    public AudioClip attackSound;

    private Collider2D enemyCollider;
    private AudioSource audioSource;
    private GameObject target;
    private Animator animator;

    public float attackTime = 1.5f;
    private float attackTimer;
    private 
    void Awake()
    {

        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.25f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(collision.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage((int)power*5, transform.position, 1);
            }
            if (collision.collider.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.GetKnockedBack((collision.transform.position - transform.position)*500, 0.2f);
            }
        }
        
    }
    
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        currentVisionDistance = visionDistance;
        //attackTimer = attackTime;
        animator.Play("PlantIdle");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        WatchTarget();
        HandleStates();
    }

    void WatchTarget()
    {
        ChangeState(EnemyState.Idle);
        Vector2 direction = target.transform.position - transform.position; // direction from enemy to the target
        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitAmount = enemyCollider.Raycast(direction, results, currentVisionDistance);
        RaycastHit2D hit = results[0];
        
        if (hit.collider != null && hit.collider.CompareTag(target.tag)) // if the raycast hits the player
        {
            if (drawDebug) Debug.DrawLine(transform.position, transform.position + (target.transform.position - transform.position).normalized * currentVisionDistance, Color.green);
            if (Vector3.Distance(transform.position, hit.collider.transform.position) < visionDistance) // if the player is close enough to attack
            {
                ChangeState(EnemyState.Follow);
                return;
            }
            


        }
        else // hits something else (wall ...)
        {
            //Debug.DrawLine(transform.position, transform.position +(target.transform.position - transform.position).normalized * visionDistance, Color.red);
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
                break;
        }


    }
    void Attack()
    {
        ChangeState(EnemyState.Attack);
        attackTimer = attackTime;
        if(!AnimatorIsPlaying()) animator.Play("PlantAttack");
        Invoke(nameof(SpawnHomingProjectile), 0.1f);
        ChangeState(EnemyState.Follow);
    }
    void FollowPlayer()
    {
        if (!AnimatorIsPlaying()) animator.Play("PlantIdle");
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Attack();
            return;
        }
    }
    void Idle()
    {
        attackTimer = attackTime;
        if (!AnimatorIsPlaying()) animator.Play("PlantIdle");
    }

    void SpawnHomingProjectile()
    {
        audioSource.PlayOneShot(attackSound);
        GameObject homing = Instantiate(homingSalivaPrefab, attackSpawnPos.position, Quaternion.identity);
        homing.transform.localScale = new Vector3(homing.transform.localScale.x/2, homing.transform.localScale.y / 2);
        HomingSaliva homingScript = homing.GetComponent<HomingSaliva>();
        homingScript.speed /= 2f;
        homingScript.maxHealth *= (int)power/4;

    }
    void ChangeState(EnemyState newState)
    {
        //Debug.Log($"Changing state from {enemyState} to {newState}");
        enemyState = newState;
    }

    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
