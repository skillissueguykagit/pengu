using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            hurtbox.TakeDamage(damage);
        }
    }
}