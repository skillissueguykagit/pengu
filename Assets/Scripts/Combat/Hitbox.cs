using UnityEngine;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private GameObject owner;

    private HashSet<Hurtbox> hitTargets = new HashSet<Hurtbox>();

    private void OnEnable()
    {
        hitTargets.Clear();

        BoxCollider2D box = GetComponent<BoxCollider2D>();

        Collider2D[] targets = Physics2D.OverlapBoxAll(
            box.bounds.center,
            box.bounds.size,
            0f
        );

        foreach (Collider2D target in targets)
        {
            TryHit(target);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryHit(other);
    }

    private void TryHit(Collider2D other)
    {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox == null)
            return;

        if (owner != null &&
            hurtbox.transform.root.gameObject == owner)
            return;

        if (hitTargets.Contains(hurtbox))
            return;

        hitTargets.Add(hurtbox);

        hurtbox.TakeDamage(damage);
    }
}