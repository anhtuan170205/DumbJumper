using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SceneLoader sceneLoader;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float groundCheckWidth = 0.5f;
    [SerializeField] private Transform groundCheckPoint;

    private Vector2 previousMoveInput;
    private bool isGrounded;

    private void Start()
    {
        inputReader.MoveEvent += HandleMove;
        inputReader.JumpEvent += HandleJump;
    }

    private void OnDestroy()
    {
        inputReader.MoveEvent -= HandleMove;
        inputReader.JumpEvent -= HandleJump;
    }

    private void FixedUpdate()
    {
        // Apply movement
        rb.velocity = new Vector2(previousMoveInput.x * moveSpeed, rb.velocity.y);
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput.magnitude > 0.1f ? moveInput.normalized : Vector2.zero;
        
        if (moveInput.x != 0)
        {
            bodyTransform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }
    }

    private void HandleJump()
    {
        if (CheckGrounded())
        {
            ExecuteJump();
        }
    }

    private void ExecuteJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CheckGrounded()
    {
        if (groundCheckPoint == null) return false;

        RaycastHit2D hit = Physics2D.BoxCast(
            groundCheckPoint.position,
            new Vector2(groundCheckWidth, 0.05f),
            0f,
            Vector2.down,
            groundCheckDistance,
            LayerMask.GetMask("Platform")
        );

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = Color.green;
        Vector3 boxCenter = groundCheckPoint.position + Vector3.down * groundCheckDistance/2;
        Gizmos.DrawWireCube(
            boxCenter,
            new Vector3(groundCheckWidth, groundCheckDistance, 0)
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Die();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        sceneLoader.LoadGameOver();
    }
}
