using UnityEngine;

public class SmoothCamera2D : MonoBehaviour
{
    public float DampTime = 0.15f;
    public Transform Target;

    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Target)
        {
            var point = mainCamera.WorldToViewportPoint(Target.position);
            var delta = Target.position - mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            var destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }

    }
}