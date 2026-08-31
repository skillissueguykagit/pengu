using UnityEngine;

public class EnemyFinish : MonoBehaviour
{
    [Header("Finish")]
    [SerializeField] private float executeHealthThreshold = 25f;

    private EnemyStates enemyStates;
    private EnemyStagger enemyStagger;
    private Health health;
    private EnemyAI enemyAI;
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        enemyStates = GetComponent<EnemyStates>();
        enemyStagger = GetComponent<EnemyStagger>();
        health = GetComponent<Health>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        if (!CanFinish())
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Execute();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Spare();
        }
    }

    private bool CanFinish()
    {
        if (enemyStates == null ||
            enemyStagger == null ||
            health == null)
        {
            return false;
        }

        if (!enemyStagger.IsStaggered())
            return false;

        if (enemyStates.GetState() !=
            EnemyStates.EnemyState.Staggered)
        {
            return false;
        }

        return health.GetCurrentHealth() <= executeHealthThreshold;
    }

    private void Execute()
    {
        enemyStates.SetState(
            EnemyStates.EnemyState.Dead
        );

        DisableEnemy();

        Debug.Log("ENEMY EXECUTED");
    }

    private void Spare()
    {
        enemyStates.SetState(
            EnemyStates.EnemyState.Spared
        );

        DisableEnemy();

        Debug.Log("ENEMY SPARED");
    }

    private void DisableEnemy()
    {
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (enemyAttack != null)
        {
            enemyAttack.CancelAttack();
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}