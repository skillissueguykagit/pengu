using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float xInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleGravity();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if ((Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.UpArrow)) &&
             isGrounded)
        {
            Jump();
        }

        // shortu jump
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

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(
            xInput * moveSpeed,
            rb.linearVelocityY
        );
    }

    private void HandleGravity()
    {
        if (rb.linearVelocityY < 0)
        {
            // fas fas fallin
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.fixedDeltaTime;
        }
        else if (rb.linearVelocityY > 0)
        {
            // fas fallin
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
}