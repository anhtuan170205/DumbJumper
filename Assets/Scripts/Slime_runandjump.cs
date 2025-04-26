using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_runandjump : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private Vector3 startPos;
    private bool movingRight = true;
    private bool isDead = false;
    private bool isGrounded = false;
    private bool isPerformingAction = false;

    private Animator anim;
    private Rigidbody2D rb;

    private enum ActionState { Idle, FastRun, JumpAndRun }
    private ActionState currentAction = ActionState.Idle;
    private bool hasJumped = false; 

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

        CheckGrounded();

        if (isGrounded && !isPerformingAction)
        {
            StartCoroutine(PerformAction());
        }

        HandleAction();
    }

    private IEnumerator PerformAction()
    {
        isPerformingAction = true;

        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        {
            currentAction = ActionState.JumpAndRun;
            hasJumped = false; // Reset mỗi lần chọn JumpAndRun
        }
        else if (randomNumber == 1)
        {
            currentAction = ActionState.FastRun;
        }

        yield return new WaitForSeconds(1f); // Thực hiện hành động trong 1 giây

        currentAction = ActionState.Idle;
        isPerformingAction = false;
    }

    private void HandleAction()
    {
        switch (currentAction)
        {
            case ActionState.FastRun:
                Move(3f);
                break;
            case ActionState.JumpAndRun:
                if (!hasJumped)
                {
                    Jump();
                    hasJumped = true; // Sau khi nhảy 1 lần thì set true
                }
                Move(2f);
                break;
            case ActionState.Idle:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Move(float speed)
    {
        float moveDirection = movingRight ? 1f : -1f;

        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);

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

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

