using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 7f;
    [SerializeField] private float invulnerabilityDuration = 0.8f;

    private Rigidbody2D rb;
    private Health health;

    private bool isInvulnerable;
    private float invulnerabilityTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (!isInvulnerable)
            return;

        invulnerabilityTimer -= Time.deltaTime;

        if (invulnerabilityTimer <= 0)
        {
            isInvulnerable = false;
        }
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isInvulnerable)
            return;

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;

        rb.linearVelocity = new Vector2(
            hitDirection.x * knockbackForce,
            hitDirection.y * knockbackForce
        );
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}