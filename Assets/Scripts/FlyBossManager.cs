using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBossManager : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] Transform groundCheckUp;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] Transform groundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool isAttacking=false;
    private int bossSpeed = 20;
    private float choosingCooldown = 2;
    private float attackDelay = 8;
    private float roomWidth = 85;
    private float roomHeight = 40;
    private SalivaFire salivaFire;
    private Animator animator;
    private float dashSpeed = 35;
    public GameObject BabyFly; // Yavru sinek prefabý
    public float rainDuration = 5f; // Sineklerin ne kadar süreyle yaðacaðý
    public float rainInterval = 0.1f; // Sineklerin arasýndaki zaman aralýðý
    private float rainHeight = 30f; // Bebek sinek saldýrýsýnýn nereden baþlayacaðý (yükseklik)



    public Rigidbody2D player;
    float distanceToCeiling;
    private Rigidbody2D enemyRB;
    public BossState currentState;


    void Start()
    {
     
        enemyRB = GetComponent<Rigidbody2D>();
        salivaFire = GetComponent<SalivaFire>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(groundCheckWall.position, groundCheckRadius, groundLayer);

        if (isAttacking == false)
        {
            ChooseAttack();

            switch (currentState)
            {
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
                    animator.Play("FlyBossDash");
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

    }


    IEnumerator UpNdownAttack()
    {
        isAttacking = true;
        if (groundCheckWall == true)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        //enemyRB.velocity = new Vector3(0, -bossSpeed, 0);
        if (isTouchingUp == true)
        {
            enemyRB.velocity = new Vector3(0, -bossSpeed, 0);
            yield return new WaitForSeconds((roomHeight/bossSpeed));
            enemyRB.velocity = Vector3.zero;
        }

        else if (isTouchingDown == true)
        {
            enemyRB.velocity = new Vector3(0, bossSpeed, 0);
            yield return new WaitForSeconds((roomHeight / bossSpeed));
            enemyRB.velocity = Vector3.zero;
        }

        else
        {
            
            enemyRB.velocity = new Vector3(0, bossSpeed, 0);
            RaycastHit2D hit = Physics2D.Raycast(enemyRB.position, Vector2.up,float.MaxValue,groundLayer);

            if (hit.collider!=null)
            {
                // Iþýn ile tavan arasýndaki mesafeyi hesapla
                distanceToCeiling = hit.distance;
            
            }

            
            yield return new WaitForSeconds(distanceToCeiling / bossSpeed);
            enemyRB.velocity=Vector3.zero;
        
          



            
        }
       




        isAttacking = false; 
    }


    IEnumerator DashAttack() {
        isAttacking = true;


        //enemyRB.velocity = new Vector3(-bossSpeed, 0, 0);
        //yield return new WaitForSeconds(roomWidth/bossSpeed);
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        ////if (isTouchingWall == true)
        ////{
        ////    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        ////    enemyRB.velocity = new Vector3(-enemyRB.velocity.x, 0, 0);

        ////}

        //enemyRB.velocity = new Vector3(bossSpeed, 0, 0);
        //yield return new WaitForSeconds(roomWidth / bossSpeed);
        //transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        float waitSecond = 1;
        Vector3 playerPosition = player.transform.position;
        Vector3 distance = (playerPosition - transform.position);
        Vector3 direction = distance.normalized;
        enemyRB.velocity = direction * (dashSpeed);
        float timer = 0;
        yield return new WaitForSeconds(waitSecond);
        while(timer<(distance.magnitude /dashSpeed)-waitSecond){
            Debug.DrawLine(transform.position, direction);
            if (isTouchingUp == true || isTouchingWall == true)
            {
                enemyRB.velocity = Vector3.zero;
                
            }
            yield return new();
            timer += Time.deltaTime;
        }
        enemyRB.velocity = -direction * (bossSpeed);
        yield return new WaitForSeconds((distance.magnitude / dashSpeed));
        yield return new WaitForSeconds(waitSecond);

        isAttacking = false;

    }
 

    IEnumerator SwipeAttack()
    {

        isAttacking = true;
        InvokeRepeating("SpawnSaliva", 1f,3f); // 2 saniyede bir spawn et
        enemyRB.velocity = new Vector3(-bossSpeed, 0, 0);
        
        //if (isTouchingWall == true)
        //{
        //    
        //   

        //}
        yield return new WaitForSeconds(roomWidth / bossSpeed);
        enemyRB.velocity = new Vector3(bossSpeed, 0, 0);
        
        yield return new WaitForSeconds(roomWidth / bossSpeed);
        CancelInvoke("SpawnSaliva");
        isAttacking = false;

    }


    IEnumerator BabyFlyAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay/2);

        float timer = 0f;

        while (timer < rainDuration)
        {
            // Yavru sinek oluþtur
            GameObject rainDrop = Instantiate(BabyFly, new Vector3(Random.Range(-50f, 50f), rainHeight,0), Quaternion.identity);

            yield return new WaitForSeconds(rainInterval); // Belirli bir süre beklet
            timer += rainInterval;
        }



        isAttacking = false;

    }

    public void SpawnSaliva()
    {
        salivaFire.SpawnSaliva();
        animator.Play("FlyBossUpDownAttack");
    }




    public enum BossState
    {
        UpNdownAttack,
        DashAttack,
        SwipeAttack,
        BabyFlyAttack,
    }
   
    void ChooseAttack()
    {   
        switch (Random.Range(0, 4))
        {
            case 0: ChangeState(BossState.UpNdownAttack); break;

            case 1: ChangeState(BossState.DashAttack); break;

            case 2: ChangeState(BossState.SwipeAttack); break;

            case 3: ChangeState(BossState.BabyFlyAttack); break;
        }

    }

    void ChangeState(BossState state) {
        currentState = state;
        
    }
}
