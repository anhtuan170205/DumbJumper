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
    private Vector2 previousMoveInput;

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
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput.normalized;
    }
    private void HandleJump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   
    }

}
