using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 1f;

    private Health health;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private PlayerDamage playerDamage;

    private Vector3 respawnPosition;

    private bool isDead;

    private void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerDamage = GetComponent<PlayerDamage>();

        respawnPosition = transform.position;
    }

    private void Update()
    {
        if (isDead)
            return;

        if (health != null && health.IsDead())
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerDamage != null)
        {
            playerDamage.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        transform.position = respawnPosition;

        if (health != null)
        {
            health.ResetHealth();
        }

        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (playerDamage != null)
        {
            playerDamage.enabled = true;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        isDead = false;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }
}