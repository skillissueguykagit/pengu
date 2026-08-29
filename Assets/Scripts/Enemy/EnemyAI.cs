using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float wallCheckDistance = 0.15f;
    [SerializeField] private float groundCheckDistance = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    private bool playerDetected;
    private float patrolDirection = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void FixedUpdate()
    {
        if (playerDetected)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void DetectPlayer()
    {
        Collider2D playerHit = Physics2D.OverlapCircle(
            transform.position,
            detectionRange,
            playerLayer
        );

        if (playerHit == null)
        {
            playerDetected = false;
            return;
        }

        player = playerHit.transform;

        Vector2 direction = (
            player.position - transform.position
        ).normalized;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        RaycastHit2D wallHit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            groundLayer
        );

        if (wallHit.collider == null)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
    }

    private void ChasePlayer()
    {
        float direction = Mathf.Sign(
            player.position.x - transform.position.x
        );

        patrolDirection = direction;

        rb.linearVelocity = new Vector2(
            direction * moveSpeed,
            rb.linearVelocityY
        );
    }

    private void Patrol()
    {
        Bounds bounds = enemyCollider.bounds;

        float frontX = patrolDirection > 0
            ? bounds.max.x
            : bounds.min.x;

        Vector2 wallOrigin = new Vector2(
            frontX + patrolDirection * 0.05f,
            bounds.center.y
        );

        RaycastHit2D wallHit = Physics2D.Raycast(
            wallOrigin,
            Vector2.right * patrolDirection,
            wallCheckDistance,
            groundLayer
        );

        Vector2 groundOrigin = new Vector2(
            frontX + patrolDirection * 0.2f,
            bounds.min.y
        );

        RaycastHit2D groundHit = Physics2D.Raycast(
            groundOrigin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (wallHit.collider != null ||
            groundHit.collider == null)
        {
            patrolDirection *= -1f;
        }

        rb.linearVelocity = new Vector2(
            patrolDirection * moveSpeed,
            rb.linearVelocityY
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        if (player != null)
        {
            Gizmos.color = playerDetected
                ? Color.green
                : Color.red;

            Gizmos.DrawLine(
                transform.position,
                player.position
            );
        }
    }
}