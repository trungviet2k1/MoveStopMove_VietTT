using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 positionOffset = new(20f, 15f, 0f);
    [SerializeField] private float smoothSpeed;

    private Vector3 lookOffset;

    void Start()
    {
        if (player == null) return;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(player.position + lookOffset);
        }
    }
}