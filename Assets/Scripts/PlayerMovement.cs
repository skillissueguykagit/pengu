using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float groundAcceleration = 55f;
    [SerializeField] private float groundDeceleration = 65f;
    [SerializeField] private float airAcceleration = 35f;
    [SerializeField] private float airDeceleration = 25f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float fallMultiplier = 2.8f;
    [SerializeField] private float lowJumpMultiplier = 2.2f;

    [Header("Jump Timing")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Collision Checks")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float groundCheckWidth = 0.8f;
    [SerializeField] private float wallCheckHeight = 0.8f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall")]
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallJumpForce = 7.5f;
    [SerializeField] private float wallJumpHorizontalForce = 7f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.1f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private float xInput;
    private float facingDirection = 1f;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isDashing;
    private bool hasDashed;

    private int wallDirection;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float dashTimeCounter;
    private float dashCooldownCounter;

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
        HandleDash();

        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            HandleDashMovement();
            return;
        }

        HandleMovement();
        HandleGravity();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (xInput != 0)
        {
            facingDirection = Mathf.Sign(xInput);
        }

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((Input.GetKeyUp(KeyCode.Space) ||
             Input.GetKeyUp(KeyCode.UpArrow)) &&
            rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryDash();
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

        if (coyoteTimeCounter > 0)
        {
            Jump();

            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            return;
        }

        if (isTouchingWall && !isGrounded)
        {
            WallJump();

            jumpBufferCounter = 0;
        }
    }

    private void HandleMovement()
    {
        float targetSpeed = xInput * moveSpeed;

        float acceleration;

        if (Mathf.Abs(xInput) > 0.01f)
        {
            acceleration = isGrounded
                ? groundAcceleration
                : airAcceleration;
        }
        else
        {
            acceleration = isGrounded
                ? groundDeceleration
                : airDeceleration;
        }

        float newSpeed = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(
            newSpeed,
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
        float jumpDirection = -wallDirection;

        rb.linearVelocity = new Vector2(
            jumpDirection * wallJumpHorizontalForce,
            wallJumpForce
        );
    }

    private void HandleWallSlide()
    {
        if (isDashing)
            return;

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

    private void TryDash()
    {
        if (isDashing)
            return;

        if (hasDashed)
            return;

        if (dashCooldownCounter > 0)
            return;

        StartDash();
    }

    private void StartDash()
    {
        isDashing = true;
        hasDashed = true;

        dashTimeCounter = dashDuration;
        dashCooldownCounter = dashCooldown;

        float dashDirection = xInput != 0
            ? Mathf.Sign(xInput)
            : facingDirection;

        rb.linearVelocity = new Vector2(
            dashDirection * dashSpeed,
            0f
        );
    }

    private void HandleDash()
    {
        if (!isDashing)
            return;

        dashTimeCounter -= Time.deltaTime;

        if (dashTimeCounter <= 0)
        {
            EndDash();
        }
    }

    private void HandleDashMovement()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );
    }

    private void EndDash()
    {
        isDashing = false;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            0f
        );
    }

    private void CheckCollisions()
    {
        Bounds bounds = playerCollider.bounds;

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

        if (isGrounded)
        {
            hasDashed = false;
        }

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