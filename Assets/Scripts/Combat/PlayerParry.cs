using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private float parryDuration = 0.15f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

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

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(ParryFlash());

        Debug.Log("PARRY SUCCESS");
    }
    private System.Collections.IEnumerator ParryFlash()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.04f);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.04f);
        }
    }
}