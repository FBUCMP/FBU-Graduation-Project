using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyBossManager : MonoBehaviour, IDamageable
{
    [Header("CornerPoints")]
    [SerializeField] Transform TopLeft;
    [SerializeField] Transform TopRight;
    [SerializeField] Transform BottomLeft;
    [SerializeField] Transform BottomRight;
    
    [SerializeField] LayerMask groundLayer;
    [Header("AudioClips")]
    [SerializeField] AudioClip screamClip;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip gooClip;
    [Header("Other")]

    public float rainDuration = 5f; // Sineklerin ne kadar süreyle yaðacaðý
    public float rainInterval = 0.1f; // Sineklerin arasýndaki zaman aralýðý
    public GameObject BabyFly; // Yavru sinek prefabý
    public Transform BabySpawnPoints; // baby fly spawn points

    public Rigidbody2D player;
    public BossState currentState;



    private Rigidbody2D enemyRB;

    private bool isAttacking=false;
    private int bossSpeed = 20;
    private float dashSpeed = 40;
    private float attackDelay = 8;
    private float roomWidth = 85;
    private float roomHeight = 40;

    private SalivaFire salivaFire;

    private Animator animator;
    private AudioSource audioSource;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    [SerializeField]
    private int _health;
    [SerializeField]
    private int _maxHealth = 8000;
    [SerializeField]
    private float _explosionEffect = 1f; // 0-1 value

    public int currentHealth { get => _health; set => _health = value; } // getter and setter
    public int maxHealth { get => _maxHealth; set => _maxHealth = value; } // getter and setter
    public float explosionEffect { get => _explosionEffect; set => _explosionEffect = value; } // getter and setter


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        roomHeight = ( (TopLeft.position.y - BottomLeft.position.y) + (TopRight.position.y - BottomRight.position.y) ) / 2;
        roomWidth = ( (TopRight.position.x - TopLeft.position.x) + (BottomRight.position.x - BottomLeft.position.x) ) / 2;
        
        if(TryGetComponent(out AudioSource audio))
        {
            audioSource = audio;
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.5f;
        }
        enemyRB = GetComponent<Rigidbody2D>();
        salivaFire = GetComponent<SalivaFire>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;

        ChangeState(BossState.Reposition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAttacking == false)
        {

            switch (currentState)
            {
                case BossState.Reposition:
                    StartCoroutine(Reposition());
                    animator.Play("FlyBossIdle");
                    break;
                case BossState.UpNdownAttack:
                    StartCoroutine(UpNdownAttack());
                    animator.Play("FlyBossUpDownAttackBegin");
                    break;
                case BossState.DashAttack:
                    StartCoroutine(DashAttack());
                    animator.Play("FlyBossDash");
                    break;
                case BossState.SwipeAttack:
                    StartCoroutine(SwipeAttack());
                    animator.Play("FlyBossSwipeBegin");
                    break;

                case BossState.BabyFlyAttack:
                    StartCoroutine(BabyFlyAttack());
                    animator.Play("FlyBossIdle");
                    break;
            }
        }

        if (enemyRB.velocity.x > 0) {
         transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
        else if(enemyRB.velocity.x < 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }

    }

    private void OnDrawGizmos()
    {
        /*
        if (isTouchingUp) { 
            Gizmos.color = Color.red;
            
        }
        else
        {
            Gizmos.color= Color.white;

        }
        Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);

        if (isTouchingDown)
        {
            Gizmos.color = Color.red;

        }
        else
        {
            Gizmos.color = Color.white;

        }
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        */
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(10, collision.collider.ClosestPoint(transform.position), 1);
            audioSource.PlayOneShot(hitClip);
        }
        if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.GetKnockedBack((collision.collider.ClosestPoint(transform.position)-(Vector2)transform.position) * 50, .5f);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.GetKnockedBack((collision.collider.ClosestPoint(transform.position) - (Vector2)transform.position) * 50, .5f);
        }
    }

    IEnumerator UpNdownAttack() // animation event handles the attack part
    {
        isAttacking = true;
        LookToPlayer();
        Vector2 closest = GetClosestCornerPoint();
        
        if (closest == (Vector2)TopRight.position || closest == (Vector2)TopRight.position) // if closest point is at top
        {
            enemyRB.velocity = new Vector3(0, -bossSpeed, 0);
            yield return new WaitForSeconds((roomHeight/bossSpeed));
            enemyRB.velocity = Vector3.zero;
        }
        else // if closest point is at bottom
        {
            enemyRB.velocity = new Vector3(0, bossSpeed, 0);
            yield return new WaitForSeconds(roomHeight / bossSpeed);
            enemyRB.velocity = Vector3.zero;
        
        }
       

        isAttacking = false;
        ChangeState(BossState.Reposition);
    }


    IEnumerator DashAttack() {
        isAttacking = true;
        LookToPlayer();
        
        float waitSecond = 1;
        Vector3 playerPosition = player.transform.position;
        Vector3 distance = (playerPosition - transform.position);
        Vector3 direction = distance.normalized;
        enemyRB.velocity = direction * (dashSpeed);
        audioSource.PlayOneShot(dashClip);
        float timer = 0;
        yield return new WaitForSeconds(waitSecond);
        while(timer<(distance.magnitude /dashSpeed)){
            Debug.DrawLine(transform.position, direction);
            
            yield return new();
            timer += Time.deltaTime;
        }

        isAttacking = false;

        ChangeState(BossState.Reposition);
    }
 

    IEnumerator SwipeAttack()
    {
        isAttacking = true;
        LookToPlayer();

        yield return new();
        animator.Play("FlyBossSwipeBegin");
        if (GetClosestCornerPoint().x > 0) // on the right side
        {
            enemyRB.velocity = new Vector3(-bossSpeed, 0, 0);
        }
        else
        {
            enemyRB.velocity = new Vector3(bossSpeed, 0, 0);
        }
        yield return new WaitForSeconds(roomWidth/bossSpeed);

        isAttacking = false;
        ChangeState(BossState.Reposition);
    }


    IEnumerator BabyFlyAttack()
    {
        isAttacking = true;
        LookToPlayer();
        audioSource.PlayOneShot(screamClip);
        yield return new WaitForSeconds(attackDelay/2);

        isAttacking = false;
        ChangeState(BossState.Reposition);

        float timer = 0f;

        while (timer < rainDuration)
        {
            // Yavru sinek oluþtur
            int index = Random.Range(0, BabySpawnPoints.childCount);
            GameObject rainDrop = Instantiate(BabyFly, BabySpawnPoints.GetChild(index).position, Quaternion.identity);
            float interval = rainInterval + Random.Range(-0.1f, 0.1f); // add random to break the pattern a bit
            yield return new WaitForSeconds(interval); // Belirli bir süre beklet
            timer += interval;
        }


        

    }

    IEnumerator Reposition()
    {
        isAttacking = true;
        Vector2 closest = GetClosestCornerPoint();       
        Debug.Log("Repositioning to " + closest);
        yield return MoveToPosition(closest);
        isAttacking = false;
        ChooseAttack(); // choses attack after repositioning
    }

    public void SpawnSaliva()
    {
        salivaFire.SpawnSaliva();
        //animator.Play("FlyBossUpDownAttack");
        audioSource.PlayOneShot(attackClip);
    }
    public void SpawnHomingSaliva()
    {
        salivaFire.SpawnHomingSaliva();
        //animator.Play("FlyBossUpDownAttack");
        audioSource.PlayOneShot(gooClip);
    }
    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, bossSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private Vector2 GetClosestCornerPoint()
    {
        // Calculate the distance to each corner point and return the closest one's position
        Vector2[] cornerPoints = new Vector2[] { TopLeft.position, TopRight.position, BottomLeft.position, BottomRight.position };
        Vector2 closest = cornerPoints[0];
        float minDistance = Vector2.Distance(transform.position, closest);
        foreach (Vector2 point in cornerPoints)
        {
            float distance = Vector2.Distance(transform.position, point);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = point;
            }
        }
        
        return closest;
    }

    void LookToPlayer()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public enum BossState
    {
        UpNdownAttack,
        DashAttack,
        SwipeAttack,
        BabyFlyAttack,
        Reposition
    }
   
    void ChooseAttack() // called after repositioning so repositioning is mandatory
    {   
        
        switch (Random.Range(0, 4))
        {
            case 0: ChangeState(BossState.UpNdownAttack); break;

            case 1: ChangeState(BossState.DashAttack); break;

            case 2: ChangeState(BossState.SwipeAttack); break;

            case 3: ChangeState(BossState.BabyFlyAttack); break;
        }

    }

    void ChangeState(BossState state)
    {
        //Debug.Log($"State changed from: {currentState} to: {state}");
        currentState = state;
        
    }

    public void TakeDamage(int damage, Vector3 hitPos, float radius)
    {
        int damageTaken = Mathf.Clamp(damage, 0, currentHealth);

        currentHealth -= damageTaken;

        if (damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if (currentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
        }
    }
}

