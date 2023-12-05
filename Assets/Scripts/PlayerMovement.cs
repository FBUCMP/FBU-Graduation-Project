using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    // dash sonrasi arkada iz birakmasi için 
    [SerializeField] private TrailRenderer tr;

    // wall sliding ve wall jumping kismi
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.15f; // ne kadar surede tekrar walljump yapacagiyla ilgili 0.1f civari biraz hýzlý oluyor ama tam Transformice'deki wj gibi hosuma gitti.
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private float velocity_test = 0f;
    [SerializeField] private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    

    //double jump
    private bool doubleJump;

    // AUDIO MANAGER
    AudioManager audioManager;

    //animator
    [SerializeField] private Transform center; // playerin orta noktasi
    private Animator lower_anim; // belden asagi
    private Animator upper_anim; // belden yukari
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //dashingPower *= Mathf.Sqrt(transform.localScale.x);
        dashingTime /= Mathf.Sqrt(transform.localScale.x);
        Debug.Log(dashingPower);
        Debug.Log(dashingTime);
        speed *= Mathf.Sqrt(transform.localScale.x);
        jumpingPower *= Mathf.Sqrt(transform.localScale.x);
        wallJumpingPower *= Mathf.Sqrt(transform.localScale.x);
        wallSlidingSpeed *= Mathf.Sqrt(transform.localScale.x);
        if (GameObject.FindGameObjectWithTag("Audio"))
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Audio taglý componenta eriþim saðlýyoruz
        }
        if (GetComponentsInChildren<Animator>().Length > 0) // upper body ve lower body childlarinin animatorleri
        {
            lower_anim = GetComponentsInChildren<Animator>()[0];
            upper_anim = GetComponentsInChildren<Animator>()[1];

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
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();

        //if (!isWallJumping) { Flip(); }

        if (lower_anim && upper_anim) // animatorler varsa
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            Vector3 lookDirection = mousePosition - center.position; // karakterin ortasindan mousea dogru bir vektor
            float angle = Mathf.Atan2(lookDirection.y, Mathf.Abs(lookDirection.x)) * Mathf.Rad2Deg; // aci hesabi, aci negatif olamaz
            //upper_anim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // upper body animasyonu mousea dogru donsun
            angle += 90; // aciyi 0-180 arasi yapmak icin
            angle = Mathf.Abs(angle);
            upper_anim.SetFloat("New Float", angle); // upper body animasyonuna aciyi gonderiyoruz. adi default new float degisebilir.

            if (horizontal != 0) // horizontal harakette kos animasyonu
            {
                lower_anim.Play("LowerBodyRun");
            }
            else // degilse dur
            {
                lower_anim.Play("LowerBodyIdle");
            }
            if (lookDirection.x < 0 ) // mouse yonune don
            {
                isFacingRight = false;
                lower_anim.SetFloat("RunDir", -1f);
                Vector3 localScale = transform.localScale;
                localScale.x = - Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }
            else if (lookDirection.x > 0)
            {
                isFacingRight = true;
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }


            if (isFacingRight == (horizontal > 0)) // ileri kosuyor
            {
                lower_anim.SetFloat("RunDir", 1f); // lower animasyonuna duz gitsin diye pozitif yolluyoruz
            }
            else // geri geri kosuyor
            {
                lower_anim.SetFloat("RunDir", -1f); // lower animasyonuna ters gitsin diye negatif yolluyoruz
            }
            
        }
        
    }

    private bool IsGrounded()
    {
        // iki noktada daire olusturup checklemek yerine box ile daha etkili bir sekilde checkliyoruz
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(boxCollider.size.x * transform.localScale.x, 0.2f * transform.localScale.y), 0f, groundLayer);
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
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
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
            // Invoke fonksiyonu, belirli bir süre sonra veya belirli bir periyotta bir fonksiyonu çaðýrmak için kullanýlýr.
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }

    }


    // Dash özelliði için Coroutine kullandýk burada karakter dash atabiliyorsa anlýk olarak yatay düzlemde dashingPower kadar 
    // |   gravity'den etkilenmeyerek ilerleyecek ve sonra hareket bittiðinde gravity tekrar olmasý gereken deðerine dönecek
    // |    
    // •--> Dash süresini ve dash dolum süresini daha iyi kontrol edebilmek adýna
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

    private void OnDrawGizmos()
    {
        //Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.1f), 0f, groundLayer);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(boxCollider.size.x * transform.localScale.x, 0.2f * transform.localScale.y));
    }
}
