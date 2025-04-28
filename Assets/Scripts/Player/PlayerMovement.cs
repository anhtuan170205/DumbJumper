using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float groundCheckWidth = 0.5f;
    [SerializeField] private Transform groundCheckPoint;
    
    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.2f;
    
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;

    private Vector2 previousMoveInput;
    private bool isGrounded;
    private float lastJumpPressedTime;
    private bool jumpBuffered;
    private float lastGroundedTime;
    private bool canCoyoteJump;

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

    private void Update()
    {
        // Movement
        rb.velocity = new Vector2(previousMoveInput.x * moveSpeed, rb.velocity.y);

        // Ground check
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.BoxCast(
            groundCheckPoint.position,
            new Vector2(groundCheckWidth, 0.05f),
            0f,
            Vector2.down,
            groundCheckDistance,
            LayerMask.GetMask("Platform")
        );

        // Coyote time tracking
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            canCoyoteJump = true;
        }
        else if (wasGrounded && !isGrounded)
        {
            // Just left the ground - start coyote time
            StartCoroutine(DisableCoyoteTimeAfterDelay());
        }

        // Check for buffered jump or coyote jump
        bool canJump = isGrounded || (canCoyoteJump && Time.time - lastGroundedTime <= coyoteTime);
        
        if (jumpBuffered && canJump)
        {
            ExecuteJump();
            jumpBuffered = false;
            canCoyoteJump = false; // Used up coyote time
        }

        // Debug drawing
        DebugDrawGroundCheck();
    }

    private IEnumerator DisableCoyoteTimeAfterDelay()
    {
        yield return new WaitForSeconds(coyoteTime);
        canCoyoteJump = false;
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput.magnitude > 0.1f ? moveInput.normalized : Vector2.zero;
        
        // Flip character based on movement direction
        if (moveInput.x != 0)
        {
            bodyTransform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }
    }

    private void HandleJump()
    {
        lastJumpPressedTime = Time.time;
        
        bool canJump = isGrounded || (canCoyoteJump && Time.time - lastGroundedTime <= coyoteTime);
        
        if (canJump)
        {
            ExecuteJump();
            canCoyoteJump = false; // Used up coyote time
        }
        else
        {
            // Buffer the jump if we're in the air
            jumpBuffered = true;
            StartCoroutine(ClearJumpBufferAfterTime());
        }
    }

    private void ExecuteJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private IEnumerator ClearJumpBufferAfterTime()
    {
        yield return new WaitForSeconds(jumpBufferTime);
        jumpBuffered = false;
    }

    private void DebugDrawGroundCheck()
    {
        if (groundCheckPoint == null) return;
        
        Debug.DrawLine(
            groundCheckPoint.position + Vector3.right * groundCheckWidth/2,
            groundCheckPoint.position + Vector3.right * groundCheckWidth/2 + Vector3.down * groundCheckDistance,
            isGrounded ? Color.green : Color.red
        );
        Debug.DrawLine(
            groundCheckPoint.position + Vector3.left * groundCheckWidth/2,
            groundCheckPoint.position + Vector3.left * groundCheckWidth/2 + Vector3.down * groundCheckDistance,
            isGrounded ? Color.green : Color.red
        );
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint == null) return;
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 boxCenter = groundCheckPoint.position + Vector3.down * groundCheckDistance/2;
        Gizmos.DrawWireCube(
            boxCenter,
            new Vector3(groundCheckWidth, groundCheckDistance, 0)
        );
    }
}