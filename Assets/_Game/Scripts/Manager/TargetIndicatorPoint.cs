using UnityEngine;

public class TargetIndicatorPoint : MonoBehaviour
{
    public Transform Target { get; private set; }
    private Camera mainCamera;
    private float offset;

    public void Initialize(Transform target, Camera cam, float indicatorOffset)
    {
        this.Target = target;
        mainCamera = cam;
        offset = indicatorOffset;
    }

    public void UpdateTargetPosition()
    {
        if (Target != null)
        {
            Vector3 targetPosition = mainCamera.WorldToScreenPoint(Target.position);

            if (targetPosition.z > 0 && !IsInCameraView(targetPosition))
            {
                transform.position = targetPosition;
                ClampToScreen();
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    bool IsInCameraView(Vector3 screenPosition)
    {
        return screenPosition.x > 0 && screenPosition.x < Screen.width &&
               screenPosition.y > 0 && screenPosition.y < Screen.height;
    }

    void ClampToScreen()
    {
        Vector3 clampPosition = transform.position;

        clampPosition.x = Mathf.Clamp(clampPosition.x, offset, Screen.width - offset);
        clampPosition.y = Mathf.Clamp(clampPosition.y, offset, Screen.height - offset);

        transform.position = clampPosition;
    }
}