using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimeCounter;
    private float attackCooldownCounter;
    private bool isAttacking;

    private void Awake()
    {
        attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (attackCooldownCounter > 0)
        {
            attackCooldownCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryAttack();
        }

        HandleAttack();
    }

    private void TryAttack()
    {
        if (isAttacking)
            return;

        if (attackCooldownCounter > 0)
            return;

        StartAttack();
    }

    private void StartAttack()
    {
        isAttacking = true;

        attackTimeCounter = attackDuration;
        attackCooldownCounter = attackCooldown;

        attackHitbox.SetActive(true);
    }

    private void HandleAttack()
    {
        if (!isAttacking)
            return;

        attackTimeCounter -= Time.deltaTime;

        if (attackTimeCounter <= 0)
        {
            EndAttack();
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        attackHitbox.SetActive(false);
    }
}