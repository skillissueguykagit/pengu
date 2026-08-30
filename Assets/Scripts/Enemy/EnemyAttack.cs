using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDuration = 0.2f;

    private float cooldownCounter;
    private bool isAttacking;
    private float attackTimer;

    private void Update()
    {
        if (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                EndAttack();
            }
        }
    }

    public bool CanAttack()
    {
        return cooldownCounter <= 0 && !isAttacking;
    }

    public void Attack()
    {
        if (!CanAttack())
            return;

        isAttacking = true;
        attackTimer = attackDuration;
        cooldownCounter = attackCooldown;

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}