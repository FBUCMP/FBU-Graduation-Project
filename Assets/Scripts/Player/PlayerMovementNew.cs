using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{

    // Movement kismi
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] public float jumpingPower = 16f;
    private bool isFacingRight = true;

    // Dash kismi
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    // dash sonrasi arkada iz birakmasi i�in 
    [SerializeField] private TrailRenderer tr;

    // wall sliding ve wall jumping kismi
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.15f; // ne kadar surede tekrar walljump yapacagiyla ilgili 0.1f civari biraz h�zl� oluyor ama tam Transformice'deki wj gibi hosuma gitti.
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private float velocity_test = 0f;
    [SerializeField] private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    
    //[SerializeField] private Transform groundCheck;

   [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform wallCheck2;
    [SerializeField] private LayerMask wallLayer;

    // erdo duvar denemesi tekrar
    [SerializeField] private Transform groundCheckR;
    [SerializeField] private Transform groundCheckL;

    public event Action OnFlipped;

    //double jump
    private bool doubleJump;

    // AUDIO MANAGER
    AudioManager audioManager;

    //animator
    [SerializeField] private Transform center; // playerin orta noktasi
    private Animator animator;
    private void Awake()
    {
        // yunus - sayisal islemler
        boxCollider = GetComponent<BoxCollider2D>();
        //dashingPower *= Mathf.Sqrt(transform.localScale.x);
        dashingTime /= Mathf.Sqrt(transform.localScale.x);
        //Debug.Log(dashingPower);
        //Debug.Log(dashingTime);
        speed *= Mathf.Sqrt(transform.localScale.x);
        jumpingPower *= Mathf.Sqrt(transform.localScale.x);
        wallJumpingPower *= Mathf.Sqrt(transform.localScale.x);
        wallSlidingSpeed *= Mathf.Sqrt(transform.localScale.x);
        if (GameObject.FindGameObjectWithTag("Audio"))
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Audio tagl� componenta eri�im sa�l�yoruz
        }
        if (GetComponentInChildren<Animator>()) // playerin childlarinda animator varsa - Sprites objesinde olmali -
        {
            animator = GetComponentInChildren<Animator>();

        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
            }

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > velocity_test)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            // tu�a bas�ld��� anda ve bas�l� tutuldu�u anda olan
            // z�plama de�i�imi
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();

        // Buraya artik anim girdigi icin ihtiyac yok
        if (!isWallJumping) { Flip(); }

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
    /*
    private bool IsGrounded()
    {
        // iki noktada daire olusturup checklemek yerine box ile daha etkili bir sekilde checkliyoruz
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(boxCollider.size.x * transform.localScale.x * 1.2f, 0.3f * transform.localScale.y), 0f, groundLayer);
    }
    */
    //erdo isgrounded
    
    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // rampa gibi �ekillerde y�r�me ve z�plama s�k�nt�s� ya�an�yordu bunun i�in yap�ld�
        return Physics2D.OverlapCircle(groundCheckR.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheckL.position, 0.2f, groundLayer);
    }


    private void FixedUpdate()
    {
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
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(wallCheck2.position, 0.2f, wallLayer);
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
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            // Invoke fonksiyonu, belirli bir s�re sonra veya belirli bir periyotta bir fonksiyonu �a��rmak i�in kullan�l�r.
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private void Flip() // calculate if flip is needed and if so flip
    {
        /* old version
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
        */

        // new version
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDirection = mousePosition - center.position; // karakterin ortasindan mousea dogru bir vektor
        if (lookDirection.x > 0 && !isFacingRight || lookDirection.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            OnFlipped?.Invoke();

        }


    }


    // Dash �zelli�i i�in Coroutine kulland�k burada karakter dash atabiliyorsa anl�k olarak yatay d�zlemde dashingPower kadar 
    // |   gravity'den etkilenmeyerek ilerleyecek ve sonra hareket bitti�inde gravity tekrar olmas� gereken de�erine d�necek
    // |    
    // �--> Dash s�resini ve dash dolum s�resini daha iyi kontrol edebilmek ad�na
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        //DASH SOUND

        if (audioManager)
        {
            audioManager.PlaySFX(audioManager.dash);    
        }

        //
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // hata veriyordu ve gerek yoktu dendi o y�zden simdilik kapattim
    /*
    private void OnDrawGizmos()
    {
        //Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.1f), 0f, groundLayer);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(boxCollider.size.x * transform.localScale.x, 0.2f * transform.localScale.y));
    }
    */
}