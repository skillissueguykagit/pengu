using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Jump Timing")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    [Header("Collision Checks")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float groundCheckWidth = 0.8f;
    [SerializeField] private float wallCheckHeight = 0.8f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallJumpForce = 6f;
    [SerializeField] private float wallJumpHorizontalForce = 5f;
    [SerializeField] private float wallJumpLockTime = 0.15f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private float xInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;

    // -1 = wall on left
    //  0 = no wall
    //  1 = wall on right
    private int wallDirection;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float wallJumpLockCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckCollisions();
        HandleInput();
        HandleCoyoteTime();
        HandleJump();
        HandleWallSlide();

        if (wallJumpLockCounter > 0)
        {
            wallJumpLockCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleGravity();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        // Jump buffer
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Short jump
        if ((Input.GetKeyUp(KeyCode.Space) ||
             Input.GetKeyUp(KeyCode.UpArrow)) &&
             rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );
        }
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        if (jumpBufferCounter <= 0)
            return;

        // Normal jump
        if (coyoteTimeCounter > 0)
        {
            Jump();

            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            return;
        }

        // Wall jump
        if (isTouchingWall &&
            !isGrounded &&
            wallJumpLockCounter <= 0)
        {
            WallJump();

            jumpBufferCounter = 0;
        }
    }

    private void HandleMovement()
    {
        if (wallJumpLockCounter > 0)
            return;

        rb.linearVelocity = new Vector2(
            xInput * moveSpeed,
            rb.linearVelocityY
        );
    }

    private void HandleGravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.fixedDeltaTime;
        }
        else if (rb.linearVelocityY > 0)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (lowJumpMultiplier - 1) *
                Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    private void WallJump()
    {
        // Direction away from the wall
        float jumpDirection = -wallDirection;

        rb.linearVelocity = new Vector2(
            jumpDirection * wallJumpHorizontalForce,
            wallJumpForce
        );

        wallJumpLockCounter = wallJumpLockTime;
    }

    private void HandleWallSlide()
    {
        if (isTouchingWall &&
            !isGrounded &&
            rb.linearVelocityY < 0)
        {
            isWallSliding = true;

            if (rb.linearVelocityY < -wallSlideSpeed)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    -wallSlideSpeed
                );
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckCollisions()
    {
        Bounds bounds = playerCollider.bounds;

        // Ground check
        Vector2 groundOrigin = new Vector2(
            bounds.center.x,
            bounds.min.y
        );

        Vector2 groundSize = new Vector2(
            groundCheckWidth,
            0.05f
        );

        RaycastHit2D groundHit = Physics2D.BoxCast(
            groundOrigin,
            groundSize,
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        isGrounded = groundHit.collider != null;

        // Right wall check
        Vector2 rightWallOrigin = new Vector2(
            bounds.max.x,
            bounds.center.y
        );

        Vector2 wallSize = new Vector2(
            0.05f,
            wallCheckHeight
        );

        RaycastHit2D rightWallHit = Physics2D.BoxCast(
            rightWallOrigin,
            wallSize,
            0f,
            Vector2.right,
            wallCheckDistance,
            groundLayer
        );

        // Left wall check
        Vector2 leftWallOrigin = new Vector2(
            bounds.min.x,
            bounds.center.y
        );

        RaycastHit2D leftWallHit = Physics2D.BoxCast(
            leftWallOrigin,
            wallSize,
            0f,
            Vector2.left,
            wallCheckDistance,
            groundLayer
        );

        // Wall result
        if (rightWallHit.collider != null)
        {
            isTouchingWall = true;
            wallDirection = 1;
        }
        else if (leftWallHit.collider != null)
        {
            isTouchingWall = true;
            wallDirection = -1;
        }
        else
        {
            isTouchingWall = false;
            wallDirection = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();

            if (playerCollider == null)
                return;
        }

        Bounds bounds = playerCollider.bounds;

        // Ground check
        Gizmos.color = Color.green;

        Vector3 groundOrigin = new Vector3(
            bounds.center.x,
            bounds.min.y,
            0f
        );

        Vector3 groundSize = new Vector3(
            groundCheckWidth,
            0.05f,
            0f
        );

        Gizmos.DrawWireCube(
            groundOrigin + Vector3.down * groundCheckDistance,
            groundSize
        );

        // Wall checks
        Gizmos.color = Color.blue;

        Vector3 rightWallOrigin = new Vector3(
            bounds.max.x + wallCheckDistance / 2f,
            bounds.center.y,
            0f
        );

        Vector3 leftWallOrigin = new Vector3(
            bounds.min.x - wallCheckDistance / 2f,
            bounds.center.y,
            0f
        );

        Vector3 wallSize = new Vector3(
            wallCheckDistance,
            wallCheckHeight,
            0f
        );

        Gizmos.DrawWireCube(
            rightWallOrigin,
            wallSize
        );

        Gizmos.DrawWireCube(
            leftWallOrigin,
            wallSize
        );
    }
}