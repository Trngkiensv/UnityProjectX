using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Position")]
    [SerializeField]
    private Vector3 offset =
        new Vector3(0f, 7f, -9f);

    [Header("Follow")]
    [SerializeField] private bool smoothFollow = false;

    [SerializeField] private float smoothTime = 0.1f;

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition =
            target.position + offset;

        if (smoothFollow)
        {
            transform.position =
                Vector3.SmoothDamp(
                    transform.position,
                    desiredPosition,
                    ref currentVelocity,
                    smoothTime
                );
        }
        else
        {
            transform.position =
                desiredPosition;
        }
    }
}