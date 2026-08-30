using UnityEngine;

public class EnemyStagger : MonoBehaviour
{
    [SerializeField] private float staggerDuration = 0.3f;

    private float staggerTimer;
    private bool isStaggered;

    private void Update()
    {
        if (!isStaggered)
            return;

        staggerTimer -= Time.deltaTime;

        if (staggerTimer <= 0)
        {
            isStaggered = false;
        }
    }

    public void Stagger()
    {
        isStaggered = true;
        staggerTimer = staggerDuration;
    }

    public bool IsStaggered()
    {
        return isStaggered;
    }
}