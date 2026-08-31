using UnityEngine;

public class EnemyReset : MonoBehaviour
{
    private Vector3 startingPosition;

    private Health health;
    private EnemyStates enemyStates;
    private EnemyStagger enemyStagger;
    private EnemyAI enemyAI;
    private EnemyAttack enemyAttack;
    private Rigidbody2D rb;

    private void Awake()
    {
        startingPosition = transform.position;

        health = GetComponent<Health>();
        enemyStates = GetComponent<EnemyStates>();
        enemyStagger = GetComponent<EnemyStagger>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAttack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void ResetEnemy()
    {
        // Make sure the enemy exists
        gameObject.SetActive(true);

        // Reset position
        transform.position = startingPosition;

        // Reset physics
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Reset health
        if (health != null)
        {
            health.ResetHealth();

            Debug.Log(
                gameObject.name +
                " reset. Health: " +
                health.GetCurrentHealth()
            );
        }

        // Reset state
        if (enemyStates != null)
        {
            enemyStates.SetState(
                EnemyStates.EnemyState.Alive
            );
        }

        // Reset stagger
        if (enemyStagger != null)
        {
            enemyStagger.SetState(
                EnemyStates.EnemyState.Alive
            );
        }

        // Reset AI
        if (enemyAI != null)
        {
            enemyAI.enabled = true;
        }

        // Cancel attack
        if (enemyAttack != null)
        {
            enemyAttack.CancelAttack();
        }
    }
}