using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimatorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isSubscribed = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        if (GameManager.CurrentGameState == GameState.InGame)
        {
            SubscribeToInput();
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        UnsubscribeFromInput();
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.InGame)
        {
            SubscribeToInput();
        }
        else
        {
            UnsubscribeFromInput();
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
        }
    }

    private void SubscribeToInput()
    {
        if (isSubscribed) return;
        inputReader.MoveEvent += HandleMove;
        inputReader.JumpEvent += HandleJump;
        isSubscribed = true;
    }

    private void UnsubscribeFromInput()
    {
        if (!isSubscribed) return;
        inputReader.MoveEvent -= HandleMove;
        inputReader.JumpEvent -= HandleJump;
        isSubscribed = false;
    }

    private void HandleMove(Vector2 moveInput)
    {
        bool isRunning = moveInput.magnitude > 0.1f;
        animator.SetBool("isRunning", isRunning);
    }

    private void HandleJump()
    {
        animator.SetBool("isJumping", true);
        StartCoroutine(ResetJumpAfterDelay());
    }

    private IEnumerator ResetJumpAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        animator.SetBool("isJumping", false);
    }
}
