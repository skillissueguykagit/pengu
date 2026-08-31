using UnityEngine;

public class EnemyFinish : MonoBehaviour
{
    [Header("Finish")]
    [SerializeField] private int executeHealthThreshold = 25;

    [Header("Input")]
    [SerializeField] private KeyCode executeKey = KeyCode.E;
    [SerializeField] private KeyCode spareKey = KeyCode.Q;

    private Health health;
    private EnemyStates enemyStates;
    private EnemyStagger enemyStagger;
    private EnemyAI enemyAI;
    private EnemyAttack enemyAttack;
    private Rigidbody2D rb;

    private bool canFinish;

    private void Awake()
    {
        health = GetComponent<Health>();
        enemyStates = GetComponent<EnemyStates>();
        enemyStagger = GetComponent<EnemyStagger>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAttack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (enemyStates == null ||
            enemyStagger == null ||
            health == null)
        {
            return;
        }

        // Only allow finishing while staggered
        if (!enemyStagger.IsStaggered())
        {
            canFinish = false;
            return;
        }

        // Enemy must be at or below the health threshold
        if (health.GetCurrentHealth() > executeHealthThreshold)
        {
            canFinish = false;
            return;
        }

        canFinish = true;

        if (Input.GetKeyDown(executeKey))
        {
            Execute();
        }
        else if (Input.GetKeyDown(spareKey))
        {
            Spare();
        }
    }

    private void Execute()
    {
        if (!canFinish)
            return;

        enemyStates.SetState(
            EnemyStates.EnemyState.Dead
        );

        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (enemyAttack != null)
        {
            enemyAttack.CancelAttack();
            enemyAttack.enabled = false;
        }

        if (enemyStagger != null)
        {
            enemyStagger.enabled = false;
        }

        // Disable all enemy colliders
        Collider2D[] colliders =
            GetComponentsInChildren<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        canFinish = false;

        Debug.Log("ENEMY EXECUTED");
    }

    private void Spare()
    {
        if (!canFinish)
            return;

        enemyStates.SetState(
            EnemyStates.EnemyState.Spared
        );

        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (enemyAttack != null)
        {
            enemyAttack.CancelAttack();
            enemyAttack.enabled = false;
        }

        if (enemyStagger != null)
        {
            enemyStagger.enabled = false;
        }

        // Keep the enemy physically present
        // but stop it from moving.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        canFinish = false;

        Debug.Log("ENEMY SPARED");
    }

    public bool CanFinish()
    {
        return canFinish;
    }
}