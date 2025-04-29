using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float groundCheckWidth = 0.5f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer = default;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleRunAnimation();
        HandleJumpAnimation();
        HandleFlip();
    }

    private void HandleFlip(){
        if (rb.velocity.x > 0.01f){
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < -0.01f){
            spriteRenderer.flipX = true;
        }
    }

    private void HandleRunAnimation(){
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.01f;
        animator.SetBool("isRunning", isRunning);
    }

    private void HandleJumpAnimation(){
        bool isGrounded = CheckGrounded();
        animator.SetBool("isJumping", !isGrounded);
    }

    private bool CheckGrounded(){
        if (groundCheckPoint == null){
            
            return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        }
        else{
            
            RaycastHit2D hit = Physics2D.BoxCast(
                groundCheckPoint.position,
                new Vector2(groundCheckWidth, 0.05f),
                0f,
                Vector2.down,
                groundCheckDistance,
                groundLayer
            );
            return hit.collider != null;
        }
    }
}
