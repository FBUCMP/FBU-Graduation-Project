using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private float velocity_test = 0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckR;
    [SerializeField] private Transform groundCheckL;
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > velocity_test)
        {
            rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y * 0.5f);
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckR.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheckL.position, 0.2f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckR.position, 0.2f);
        Gizmos.DrawWireSphere(groundCheckL.position, 0.2f);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

}
