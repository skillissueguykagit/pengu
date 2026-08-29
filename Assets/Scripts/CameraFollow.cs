using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private float horizontalDeadZone = 2f;
    [SerializeField] private float verticalDeadZone = 1f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = player.position;

        float horizontalDifference = playerPosition.x - cameraPosition.x;
        float verticalDifference = playerPosition.y - cameraPosition.y;

        // Dead zone horiz
        if (Mathf.Abs(horizontalDifference) > horizontalDeadZone)
        {
            cameraPosition.x = playerPosition.x -
                Mathf.Sign(horizontalDifference) * horizontalDeadZone;
        }

        // Dead zone vert
        if (Mathf.Abs(verticalDifference) > verticalDeadZone)
        {
            cameraPosition.y = playerPosition.y -
                Mathf.Sign(verticalDifference) * verticalDeadZone;
        }

        cameraPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            cameraPosition,
            ref velocity,
            smoothTime
        );
    }
}