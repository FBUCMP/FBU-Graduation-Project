using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Playables;
using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{
    //Ability Dash
    public float movementInput;

    // Movement kismi
    [SerializeField] private PlayerAction playerAction;
    private float horizontal;
    [SerializeField] public float speed = 8f;
    public float jumpHeight = 20f;
    private float jumpVelocity;
    private bool isFacingRight = true;


    public bool isDashing;

    // 25.02.2024
    // cyoto time and jump buffering
    private float coyoteTime = 0.4f; // test için 2f verildi def 0.2f ayarlanabilir
    private float coyoteCounter;

    private float jumpBufferTime = 0.3f; // test için 3f verildi def 0.2f ayarlanabilir
    private float jumpBufferCounter;


    // 25.02.2024 end

    // wall sliding ve wall jumping kismi
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.15f; // ne kadar surede tekrar walljump yapacagiyla ilgili 0.1f civari biraz hýzlý oluyor ama tam Transformice'deki wj gibi hosuma gitti.
    private Vector2 walljumpVelocity = new Vector2(8f, 16f);

    [SerializeField] private float velocity_test = 0f;
    [SerializeField] public Rigidbody2D rb;
    

    private BoxCollider2D boxCollider;

    //[SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheckR;
    [SerializeField] private Transform wallCheckL;
    [SerializeField] private LayerMask wallLayer;

    // erdo duvar denemesi tekrar
    [SerializeField] private Transform groundCheckR;
    [SerializeField] private Transform groundCheckM;
    [SerializeField] private Transform groundCheckL;

    public event Action OnFlipped;

    //double jump
    public bool doubleJump;

    // AUDIO MANAGER
    AudioManager audioManager;

    //animator
    [SerializeField] private Transform center; // playerin orta noktasi
    private Animator animator;


    private void Awake()
    {
        // yunus - sayisal islemler
        boxCollider = GetComponent<BoxCollider2D>();
     
        speed *= Mathf.Sqrt(transform.localScale.x);
        //jumpVelocity *= Mathf.Sqrt(transform.localScale.x);
        jumpVelocity = Mathf.Sqrt(-2f * jumpHeight * Physics2D.gravity.y); // initial vel^2 = -2 * g * h

        walljumpVelocity *= Mathf.Sqrt(transform.localScale.x);
        wallSlidingSpeed *= Mathf.Sqrt(transform.localScale.x);
        if (GameObject.FindGameObjectWithTag("Audio"))
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Audio taglý componenta eriþim saðlýyoruz
        }
        if (GetComponentInChildren<Animator>()) // playerin childlarinda animator varsa - Sprites objesinde olmali -
        {
            animator = GetComponentInChildren<Animator>();

        }

    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0.1f;
        }
        playerAction.isStopped = false;

        movementInput = horizontal;

        // ************* DASH **************
        if (isDashing)
        {
            return;
        }
        


        horizontal = Input.GetAxisRaw("Horizontal");

        //-------------------------------------- coyote time and jump buffer
        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //-------------------------------------- 

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (jumpBufferCounter > 0f)
        {
            if (coyoteCounter > 0f || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

                doubleJump = !doubleJump;

                jumpBufferCounter = 0f;
            }

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > velocity_test)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            // tuþa basýldýðý anda ve basýlý tutulduðu anda olan
            // zýplama deðiþimi

            coyoteCounter = 0f;
        }
        



        WallSlide();
        WallJump();

        // Buraya artik anim girdigi icin ihtiyac yok
        if (!isWallJumping) { TryFlip(); }

        if (animator) // animator varsa
        {
            // TODO: mousea bakma yeniden yazilacak
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDirection = mousePosition - center.position; // karakterin ortasindan mousea dogru bir vektor
            float angle = Mathf.Atan2(lookDirection.y, Mathf.Abs(lookDirection.x)) * Mathf.Rad2Deg; // aci hesabi, aci negatif olamaz
            angle += 90; // aciyi 0-180 arasi yapmak icin
            angle = Mathf.Abs(angle);

            if (!IsGrounded() || isDashing)
            {
                animator.Play("LowerInAir");
                return;
            }
            else if (horizontal != 0) // horizontal harakette kos animasyonu
            {
                animator.Play("LowerRun");
            }
            else // degilse dur
            {
                animator.Play("LowerIdle");
            }


            if (isFacingRight == (horizontal > 0)) // ileri kosuyor
            {
                animator.SetFloat("RunDir", 1f); // lower animasyonuna duz gitsin diye pozitif yolluyoruz
            }
            else // geri geri kosuyor
            {
                animator.SetFloat("RunDir", -1f); // lower animasyonuna ters gitsin diye negatif yolluyoruz
            }


        }

    }

    // yunus isgrounded

    private bool IsGrounded()
    {
        // iki noktada daire olusturup checklemek yerine box ile daha etkili bir sekilde checkliyoruz
        return Physics2D.OverlapBox(groundCheckM.position, new Vector2(boxCollider.size.x * transform.localScale.x * 1f, 1.5f * transform.localScale.y), 0f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        //Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.1f), 0f, groundLayer);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckM.position, new Vector2(boxCollider.size.x * transform.localScale.x * 1f, 1.5f * transform.localScale.y));
        if (IsWalled())
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(wallCheckL.position, 0.2f);
            Gizmos.DrawWireSphere(wallCheckR.position, 0.2f);
        }
    }


    private void FixedUpdate()
    {
        // ************* DASH **************
        if (isDashing)
        {
            return;
        }
        
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheckL.position, .2f, wallLayer) || Physics2D.OverlapCircle(wallCheckR.position, .2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            //--------------------------------------
            doubleJump = !doubleJump;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * walljumpVelocity.x, walljumpVelocity.y);
            wallJumpingCounter = 0f;
            playerAction.isStopped = true;
            //if (transform.localScale.x != wallJumpingDirection)
            //{
            //    Flip();
            //}
            // Invoke fonksiyonu, belirli bir süre sonra veya belirli bir periyotta bir fonksiyonu çaðýrmak için kullanýlýr.
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        playerAction.isStopped = false;
        isWallJumping = false;
    }


    private void TryFlip() // calculate if flip is needed and if so call flip
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDirection = mousePosition - center.position; // karakterin ortasindan mousea dogru bir vektor
        if (lookDirection.x > 0 && !isFacingRight || lookDirection.x < 0 && isFacingRight)
        {
            Flip();
        }


    }
    private void Flip() // actual flip function
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;

        OnFlipped?.Invoke();
    }
}

