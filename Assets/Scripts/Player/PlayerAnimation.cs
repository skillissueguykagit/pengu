using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        float horizontalSpeed = rb.linearVelocityX;
        float verticalSpeed = rb.linearVelocityY;

        // Walking
        bool isWalking = Mathf.Abs(horizontalSpeed) > 0.1f;
        animator.SetBool("IsWalking", isWalking);

        // Grounded state
        bool isGrounded = playerMovement.IsGrounded();
        animator.SetBool("IsJumping", !isGrounded);

        // Dashing state
        bool isDashing = playerMovement.IsDashing();
        animator.SetBool("IsDashing", isDashing);

        // Wall sliding
        bool isWallSliding = playerMovement.IsWallSliding();
        animator.SetBool("IsWallSliding", isWallSliding);

        // Vertical velocity
        animator.SetFloat("VerticalVelocity", verticalSpeed);

        // Face direction
        if (isWallSliding)
        {
            int wallDirection = playerMovement.GetWallDirection();
            spriteRenderer.flipX = wallDirection > 0;
        }
        else
        {
            if (horizontalSpeed > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalSpeed < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}