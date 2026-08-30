using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private EnemyStagger enemyStagger;

    public void TakeDamage(int damage)
    {
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        if (enemyStagger != null)
        {
            enemyStagger.Stagger();
        }
    }
}