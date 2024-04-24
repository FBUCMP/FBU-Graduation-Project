using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBossManager : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] Transform goundCheckUp;
    [SerializeField] Transform goundCheckDown;
    [SerializeField] Transform goundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool isAttacking;

   


    private Rigidbody2D enemyRB;
    public BossState currentState;


    void Start()
    {
     
        enemyRB = GetComponent<Rigidbody2D>();
        ChooseAttack();
      
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingUp = Physics2D.OverlapCircle(goundCheckUp.position, groundCheckRadius, groundLayer);
        isTouchingDown = Physics2D.OverlapCircle(goundCheckDown.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(goundCheckWall.position, groundCheckRadius, groundLayer);
        UpNdownAttack();
    }

    void UpNdownAttack()
    {
        isAttacking = true;
        enemyRB.velocity = new Vector3(0, -5, 0);
        isAttacking = false; 
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
