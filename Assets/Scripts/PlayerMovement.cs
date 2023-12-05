using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/* 
	Player movement i�in farkl� y�ntemler:
	1- Input manager kullanma
	2- Rigidbody fizik tabanl� kontroller
	3- transform ile do�rudan kontrol
	4- animasyon ve state machine ile hareket sa�lama

	Biz �uan rigidbody ve input manager kullan�yoruz, ileride New input system package'�na ge�mek isteyebiliriz.
	
	
	Mathf.Clamp, belirli bir de�eri belirli bir aral�k i�inde s�n�rlamak i�in kullan�l�r.
	Mathf.Clamp(de�er, minimum, maksimum);
	
	
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
    private float wallJumpingDuration = 0.15f; // ne kadar surede tekrar walljump yapacagiyla ilgili 0.1f civari biraz h�zl� oluyor ama tam Transformice'deki wj gibi hosuma gitti.
    private Vector2 wallJumpingPower = new Vector2(8f, 16f); // bu k�s�m speed ve jumpingPower degerlerini al�yor

    //--------------------------------------------------------
    [SerializeField] private float velocity_test = 0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck; // rampalarda sorun ya�amamak i�in sa�a yak�n ve sola yak�n olarak 2 tane gCheck var
    [SerializeField] private Transform groundCheck1;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck; // duvarlar� tan�mam�z i�in
    [SerializeField] private LayerMask wallLayer;



    //double jump
    private bool doubleJump;

    // AUDIO MANAGER
    AudioManager audioManager;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Audio"))
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Audio tagl� componenta eri�im sa�l�yoruz
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isDashing) // Karakter dash durumundaysa bu k�sm�n atlanmas� i�in
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal"); // oyuncudan al�nan yatay giri� de�eri

        if (IsGrounded() && !Input.GetButton("Jump")) // oyuncumuz yerde ve z�plam�yorsa dJump yapmaz
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump")) // oyuncu z�plad�ysa
        {
            if (IsGrounded() || doubleJump) // yerde veya double jump halindeyse
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower); // z�plad�

                doubleJump = !doubleJump; // double jump yapt�/yapmad�
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
            StartCoroutine(Dash()); //dash coroutine ba�lang�c�
        }

        WallSlide();
        WallJump();

        if (!isWallJumping) { Flip(); } // e�er duvarda z�plama modunda de�ilse karakterin y�z�n� ayarlamas� i�in
    }

    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // rampa gibi �ekillerde y�r�me ve z�plama s�k�nt�s� ya�an�yordu bunun i�in yap�ld�
        // 2 y�nl� gCheck yap�l�yor
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheck1.position, 0.2f, groundLayer);
    }

    private void FixedUpdate() //Fiziksel i�lemlerin �al��t��� anlar�n g�ncellenmesi i�in kullan�l�yor
    {
        if (isDashing) // e�er dash atabiliyorsak bu k�s�m (FixedUpdate) atlan�r
        {
            return;
        }
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); // wall jump yoksa yatayda h�z� ayarlar
        }

    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        // 0.2f >> �emberin yar��ap�
        // duvar�n de�erini d�nd�r�r
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // a�a�� kay�� h�z�n� ayarlamak i�in kullan�ld�
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
            isWallJumping = false; // z�plama yaparken kayma olmamal�
            wallJumpingDirection = -transform.localScale.x; // y�z�n d�n�� y�n�n� belirler
            wallJumpingCounter = wallJumpingTime; // duvardan z�plama i�in belirlenen s�reyi temsil ediyor
                                                  // Walljumping counter wj �zelli�ini kontrol eden de�i�ken olarak s�ylenebilir.

            CancelInvoke(nameof(StopWallJumping)); // wjyi durdurmak
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime; // zamana ba�l� olarak wj olay�n� bitirir
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            // wj hareketinde rb de�erlerini ayarl�yoruz
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;


            if (transform.localScale.x != wallJumpingDirection)
            {
                // wj s�ras�nda duvardan ayr�l�p tekrar duvara gitmemizi sa�l�yor kabaca
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



    private void Flip()
    {
        /*
		Karakterin y�z� sa�a d�n�kse veya sola d�n�kse klavyeden gelen de�ere g�re y�z�n� do�ru y�ne �evirmesini sa�lar
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
    // Dash �zelli�i i�in Coroutine kulland�k burada karakter dash atabiliyorsa anl�k olarak yatay d�zlemde dashingPower kadar 
    // |   gravity'den etkilenmeyerek ilerleyecek ve sonra hareket bitti�inde gravity tekrar olmas� gereken de�erine d�necek
    // |    
    // �--> Dash s�resini ve dash dolum s�resini daha iyi kontrol edebilmek ad�na
	*/
    private IEnumerator Dash()
    {

        /*
			dash yapabildi�imiz anda gravityi 0'a �ekip rb gravitysini de bir de�erde tutuyoruz.
			dash atabilir oldu�umuzda oyuncu yatay d�zlemde dash power ve dash time kadar hareketi ger�ekle�tiriyor
			ard�ndan rb de�erleri eski haline geliyor.
			dash atabilme s�resini ve dash bekleme s�resini coroutine sayesinde sa�l�yoruz
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

        // coroutine yield return ile kullan�l�r
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
