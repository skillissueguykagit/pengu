using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float horizontalDeadZone = 2f;
    [SerializeField] private float verticalDeadZone = 1f;

    [Header("Look Ahead")]
    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float lookAheadSmoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;
    private float currentLookAhead;
    private float lookAheadVelocity;
    private float lastPlayerX;

    private void Start()
    {
        lastPlayerX = player.position.x;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = player.position;

        float playerMovement = playerPosition.x - lastPlayerX;

        float movementDirection = 0f;

        if (Mathf.Abs(playerMovement) > 0.001f)
        {
            movementDirection = Mathf.Sign(playerMovement);
        }

        float targetLookAhead = movementDirection * lookAheadDistance;

        currentLookAhead = Mathf.SmoothDamp(
            currentLookAhead,
            targetLookAhead,
            ref lookAheadVelocity,
            lookAheadSmoothTime
        );

        float targetX = playerPosition.x + currentLookAhead;

        float horizontalDifference =
            targetX - cameraPosition.x;

        float verticalDifference =
            playerPosition.y - cameraPosition.y;

        if (Mathf.Abs(horizontalDifference) > horizontalDeadZone)
        {
            cameraPosition.x =
                targetX -
                Mathf.Sign(horizontalDifference) *
                horizontalDeadZone;
        }

        if (Mathf.Abs(verticalDifference) > verticalDeadZone)
        {
            cameraPosition.y =
                playerPosition.y -
                Mathf.Sign(verticalDifference) *
                verticalDeadZone;
        }

        cameraPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            cameraPosition,
            ref velocity,
            smoothTime
        );

        lastPlayerX = playerPosition.x;
    }
}