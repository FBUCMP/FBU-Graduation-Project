using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/* 
	Player movement için farklý yöntemler:
	1- Input manager kullanma
	2- Rigidbody fizik tabanlý kontroller
	3- transform ile doðrudan kontrol
	4- animasyon ve state machine ile hareket saðlama

	Biz þuan rigidbody ve input manager kullanýyoruz, ileride New input system package'ýna geçmek isteyebiliriz.
	
	
	Mathf.Clamp, belirli bir deðeri belirli bir aralýk içinde sýnýrlamak için kullanýlýr.
	Mathf.Clamp(deðer, minimum, maksimum);
	
	
*/
public class PlayerMovement : MonoBehaviour
{

    // Movement part
    // Yatayda hareket
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] public float jumpingPower = 16f;
    private bool isFacingRight = true;

    // Dash part
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    // Dash trailer
    [SerializeField] private TrailRenderer tr;

    // wall sliding ve wall jumping part
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.15f; // ne kadar surede tekrar walljump yapacagiyla ilgili 0.1f civari biraz hýzlý oluyor ama tam Transformice'deki wj gibi hosuma gitti.
    private Vector2 wallJumpingPower = new Vector2(8f, 16f); // bu kýsým speed ve jumpingPower degerlerini alýyor

    //--------------------------------------------------------
    [SerializeField] private float velocity_test = 0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck; // rampalarda sorun yaþamamak için saða yakýn ve sola yakýn olarak 2 tane gCheck var
    [SerializeField] private Transform groundCheck1;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck; // duvarlarý tanýmamýz için
    [SerializeField] private LayerMask wallLayer;



    //double jump
    private bool doubleJump;

    // AUDIO MANAGER
    AudioManager audioManager;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Audio"))
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Audio taglý componenta eriþim saðlýyoruz
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isDashing) // Karakter dash durumundaysa bu kýsmýn atlanmasý için
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal"); // oyuncudan alýnan yatay giriþ deðeri

        if (IsGrounded() && !Input.GetButton("Jump")) // oyuncumuz yerde ve zýplamýyorsa dJump yapmaz
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump")) // oyuncu zýpladýysa
        {
            if (IsGrounded() || doubleJump) // yerde veya double jump halindeyse
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower); // zýpladý

                doubleJump = !doubleJump; // double jump yaptý/yapmadý
            }

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > velocity_test)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            // tuþa basýldýðý anda ve basýlý tutulduðu anda olan
            // zýplama deðiþimi
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash()); //dash coroutine baþlangýcý
        }

        WallSlide();
        WallJump();

        if (!isWallJumping) { Flip(); } // eðer duvarda zýplama modunda deðilse karakterin yüzünü ayarlamasý için
    }

    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // rampa gibi þekillerde yürüme ve zýplama sýkýntýsý yaþanýyordu bunun için yapýldý
        // 2 yönlü gCheck yapýlýyor
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheck1.position, 0.2f, groundLayer);
    }

    private void FixedUpdate() //Fiziksel iþlemlerin çalýþtýðý anlarýn güncellenmesi için kullanýlýyor
    {
        if (isDashing) // eðer dash atabiliyorsak bu kýsým (FixedUpdate) atlanýr
        {
            return;
        }
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); // wall jump yoksa yatayda hýzý ayarlar
        }

    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        // 0.2f >> çemberin yarýçapý
        // duvarýn deðerini döndürür
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // aþaðý kayýþ hýzýný ayarlamak için kullanýldý
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
            isWallJumping = false; // zýplama yaparken kayma olmamalý
            wallJumpingDirection = -transform.localScale.x; // yüzün dönüþ yönünü belirler
            wallJumpingCounter = wallJumpingTime; // duvardan zýplama için belirlenen süreyi temsil ediyor
                                                  // Walljumping counter wj özelliðini kontrol eden deðiþken olarak söylenebilir.

            CancelInvoke(nameof(StopWallJumping)); // wjyi durdurmak
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime; // zamana baðlý olarak wj olayýný bitirir
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            // wj hareketinde rb deðerlerini ayarlýyoruz
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;


            if (transform.localScale.x != wallJumpingDirection)
            {
                // wj sýrasýnda duvardan ayrýlýp tekrar duvara gitmemizi saðlýyor kabaca
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
        /*
		Karakterin yüzü saða dönükse veya sola dönükse klavyeden gelen deðere göre yüzünü doðru yöne çevirmesini saðlar
		*/
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }

    }

    /*
    // Dash özelliði için Coroutine kullandýk burada karakter dash atabiliyorsa anlýk olarak yatay düzlemde dashingPower kadar 
    // |   gravity'den etkilenmeyerek ilerleyecek ve sonra hareket bittiðinde gravity tekrar olmasý gereken deðerine dönecek
    // |    
    // •--> Dash süresini ve dash dolum süresini daha iyi kontrol edebilmek adýna
	*/
    private IEnumerator Dash()
    {

        /*
			dash yapabildiðimiz anda gravityi 0'a çekip rb gravitysini de bir deðerde tutuyoruz.
			dash atabilir olduðumuzda oyuncu yatay düzlemde dash power ve dash time kadar hareketi gerçekleþtiriyor
			ardýndan rb deðerleri eski haline geliyor.
			dash atabilme süresini ve dash bekleme süresini coroutine sayesinde saðlýyoruz
		*/
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

        // coroutine yield return ile kullanýlýr
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        Gizmos.DrawWireSphere(groundCheck1.position, 0.2f);
        Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
    }
}
