using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void HandleMove(Vector2 moveInput)
    {
        if (SceneLoader.isPaused)
        {
            return;
        }
        bool isRunning = moveInput.magnitude > 0.1f;
        animator.SetBool("isRunning", isRunning);
    }

    private void HandleJump()
    {
        if (SceneLoader.isPaused)
        {
            return;
        }
        animator.SetBool("isJumping", true);
        Invoke(nameof(ResetJump), 0.2f);
    }

    private void ResetJump()
    {
        animator.SetBool("isJumping", false);
    }
}
