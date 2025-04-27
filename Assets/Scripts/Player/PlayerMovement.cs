using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float groundCheckWidth = 0.5f;
    [SerializeField] private Transform groundCheckPoint; // Drag a child GameObject here
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
    private void Update()
    {
        rb.velocity = new Vector2(previousMoveInput.x * moveSpeed, rb.velocity.y);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Platform"));
        isGrounded = Physics2D.BoxCast(
            groundCheckPoint.position,
            new Vector2(groundCheckWidth, 0.05f), // Wider X dimension
            0f,
            Vector2.down,
            groundCheckDistance,
            LayerMask.GetMask("Platform")
        );
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

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput.normalized;
    }
    private void HandleJump()
    {
        if (!isGrounded)
        {
            return;
        }
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint == null) return;
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        // Draw the full width check area
        Vector3 boxCenter = groundCheckPoint.position + Vector3.down * groundCheckDistance/2;
        Gizmos.DrawWireCube(
            boxCenter,
            new Vector3(groundCheckWidth, groundCheckDistance, 0)
        );
    }
}
