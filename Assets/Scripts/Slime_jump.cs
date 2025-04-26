using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_jump : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float distance = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius = 0.1f;
    
    private Vector3 startPos;
    private bool movingRight = true;
    private bool isDead = false;
    private bool isGrounded = false;
    private bool shouldJump = false;
    
    private Animator anim;
    private Rigidbody2D rb;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.gravityScale = 2f; 
        }
    }
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        if (isDead) return;

        shouldJump = true;

        CheckGrounded();
        
        if (isGrounded)
        {
            Move();
            if (shouldJump)
            {
                Jump();
                shouldJump = false;
            }
        }
    }
    
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    private void Move()
    {
        float moveDirection = movingRight ? 1f : -1f;
        
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        
        if (movingRight && transform.position.x >= startPos.x + distance)
        {
            movingRight = false;
            Flip();
        }
        else if (!movingRight && transform.position.x <= startPos.x - distance)
        {
            movingRight = true;
            Flip();
        }
    }
    
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    
    private void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && !isDead)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isDead = true;
        anim.SetBool("Die", true);
        rb.velocity = Vector2.zero;
        StartCoroutine(DestroyAfterAnimation());
    }
    
    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
    // Vẽ gizmo để hiển thị điểm kiểm tra mặt đất trong Editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}