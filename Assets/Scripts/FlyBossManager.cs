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
                    animator.Play("FlyBossIdle");
                    break;
                case BossState.DashAttack:
                    StartCoroutine(DashAttack());
                    animator.Play("FlyBossDash");
                    break;
                case BossState.SwipeAttack:
                    StartCoroutine(SwipeAttack());
                    animator.Play("FlyBossDash");
                    break;
            }
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
        
        
        //enemyRB.velocity = new Vector3(0, -bossSpeed, 0);
        if (isTouchingUp == true)
        {
            enemyRB.velocity = new Vector3(0, -bossSpeed, 0);

        }

        else if (isTouchingDown == true)
        {
            enemyRB.velocity = new Vector3(0, bossSpeed, 0);
        }



        //yield return new WaitForSeconds(roomHeight / bossSpeed);
        //enemyRB.velocity = new Vector3(0, bossSpeed, 0);

        yield return new WaitForSeconds(roomHeight / bossSpeed);
       


        isAttacking = false; 
    }


    IEnumerator DashAttack() {
        isAttacking = true;


        enemyRB.velocity = new Vector3(-bossSpeed, 0, 0);
        yield return new WaitForSeconds(roomWidth/bossSpeed);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        
        //if (isTouchingWall == true)
        //{
        //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //    enemyRB.velocity = new Vector3(-enemyRB.velocity.x, 0, 0);

        //}
        
        enemyRB.velocity = new Vector3(bossSpeed, 0, 0);
        yield return new WaitForSeconds(roomWidth / bossSpeed);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        
        isAttacking = false;

    }
 

    IEnumerator SwipeAttack()
    {

        isAttacking = true;
        InvokeRepeating("SpawnSaliva", 0f, 2f); // 2 saniyede bir spawn et
        enemyRB.velocity = new Vector3(-bossSpeed, 0, 0);
        
        //if (isTouchingWall == true)
        //{
        //    
        //   

        //}
        yield return new WaitForSeconds(roomWidth / bossSpeed);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        enemyRB.velocity = new Vector3(bossSpeed, 0, 0);
        
        yield return new WaitForSeconds(roomWidth / bossSpeed);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        salivaFire.CancelInvoke("SpawnSaliva");
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
    }
   
    void ChooseAttack()
    {
        switch (Random.Range(0, 3))
        {
            case 0: ChangeState(BossState.UpNdownAttack); break;

            case 1: ChangeState(BossState.DashAttack); break;

            case 2: ChangeState(BossState.SwipeAttack); break;
        }
    }

    void ChangeState(BossState state) {
        currentState = state;
        
    }
}
