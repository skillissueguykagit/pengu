using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Health health;

    public void TakeDamage(int damage)
    {
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}