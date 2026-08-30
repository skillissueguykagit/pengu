using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private float parryDuration = 0.15f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isParrying;
    private float parryTimer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartParry();
        }

        if (!isParrying)
            return;

        parryTimer -= Time.deltaTime;

        if (parryTimer <= 0)
        {
            isParrying = false;

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    private void StartParry()
    {
        if (isParrying)
            return;

        isParrying = true;
        parryTimer = parryDuration;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        Debug.Log("PARRY WINDOW");
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public void EndParry()
    {
        if (!isParrying)
            return;

        isParrying = false;
        parryTimer = 0f;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        Debug.Log("PARRY SUCCESS");
    }
}