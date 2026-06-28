using UnityEngine;

namespace KineticStudios.CameraSystem
{
    /// <summary>
    /// Provides mouse-driven orbit, pan, and zoom navigation for the desktop studio viewport.
    /// </summary>
    public sealed class StudioCameraController : MonoBehaviour
    {
        [Header("Focus")]
        [SerializeField] private Vector3 focusPoint = new(0f, 1f, 0f);
        [SerializeField, Min(0.5f)] private float distance = 11f;

        [Header("Orbit")]
        [SerializeField] private float yaw = 135f;
        [SerializeField] private float pitch = 28f;
        [SerializeField] private float orbitSensitivity = 0.22f;
        [SerializeField] private Vector2 pitchLimits = new(-10f, 80f);

        [Header("Pan and zoom")]
        [SerializeField] private float panSensitivity = 0.002f;
        [SerializeField] private float zoomSensitivity = 1.1f;
        [SerializeField] private Vector2 distanceLimits = new(2.5f, 35f);

        [Header("UI safe area")]
        [SerializeField] private float leftPanelWidth = 264f;
        [SerializeField] private float rightPanelWidth = 320f;
        [SerializeField] private float topBarHeight = 56f;
        [SerializeField] private float bottomPanelHeight = 128f;

        private void Awake()
        {
            ApplyTransform();
        }

        private void Update()
        {
            if (!IsPointerInViewport())
            {
                return;
            }

            if (Input.GetMouseButton(1))
            {
                yaw += Input.GetAxisRaw("Mouse X") * orbitSensitivity * 10f;
                pitch -= Input.GetAxisRaw("Mouse Y") * orbitSensitivity * 10f;
                pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);
            }

            if (Input.GetMouseButton(2))
            {
                float scale = panSensitivity * distance;
                Vector3 offset = (-transform.right * Input.GetAxisRaw("Mouse X")
                                  - transform.up * Input.GetAxisRaw("Mouse Y")) * scale;
                focusPoint += offset;
            }

            float scroll = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(scroll, 0f))
            {
                distance = Mathf.Clamp(distance - scroll * zoomSensitivity, distanceLimits.x, distanceLimits.y);
            }

            ApplyTransform();
        }

        private bool IsPointerInViewport()
        {
            Vector3 pointer = Input.mousePosition;
            return pointer.x >= leftPanelWidth
                   && pointer.x <= Screen.width - rightPanelWidth
                   && pointer.y >= bottomPanelHeight
                   && pointer.y <= Screen.height - topBarHeight;
        }

        private void ApplyTransform()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            transform.SetPositionAndRotation(focusPoint - rotation * Vector3.forward * distance, rotation);
        }
    }
}
