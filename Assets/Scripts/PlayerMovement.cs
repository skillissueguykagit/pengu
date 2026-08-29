using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 4f;
    public Rigidbody2D rb;
    private float xInput;
    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
    }
}
