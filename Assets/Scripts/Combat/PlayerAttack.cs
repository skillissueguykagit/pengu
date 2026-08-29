using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDuration = 0.15f;
    [SerializeField] private float attackCooldown = 0.3f;

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

        if (Input.GetKeyDown(KeyCode.X))
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