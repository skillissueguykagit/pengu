using UnityEngine;

public class EnemyStagger : MonoBehaviour
{
    [Header("Stagger")]
    [SerializeField] private float staggerDuration = 1.5f;

    private float staggerTimer;

    private EnemyStates enemyStates;
    private EnemyAI enemyAI;
    private EnemyAttack enemyAttack;
    private Rigidbody2D rb;

    private void Awake()
    {
        enemyStates = GetComponent<EnemyStates>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAttack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsStaggered())
            return;

        staggerTimer -= Time.deltaTime;

        if (staggerTimer <= 0)
        {
            SetState(EnemyStates.EnemyState.Alive);
        }
    }

    private void FixedUpdate()
    {
        if (!IsStaggered())
            return;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void Stagger()
    {
        if (enemyStates == null)
            return;

        EnemyStates.EnemyState currentState =
            enemyStates.GetState();

        if (currentState == EnemyStates.EnemyState.Dead ||
            currentState == EnemyStates.EnemyState.Spared)
        {
            return;
        }

        enemyStates.SetState(
            EnemyStates.EnemyState.Staggered
        );

        staggerTimer = staggerDuration;

        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (enemyAttack != null)
        {
            enemyAttack.CancelAttack();
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public bool IsStaggered()
    {
        if (enemyStates == null)
            return false;

        return enemyStates.GetState() ==
               EnemyStates.EnemyState.Staggered;
    }

    public void SetState(EnemyStates.EnemyState newState)
    {
        if (enemyStates == null)
            return;

        enemyStates.SetState(newState);

        if (newState == EnemyStates.EnemyState.Alive)
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            if (enemyAI != null)
            {
                enemyAI.enabled = true;
            }
        }
        else
        {
            if (enemyAI != null)
            {
                enemyAI.enabled = false;
            }

            if (enemyAttack != null)
            {
                enemyAttack.CancelAttack();
            }
        }
    }
}