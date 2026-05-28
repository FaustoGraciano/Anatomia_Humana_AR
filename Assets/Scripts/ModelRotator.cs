using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ModelRotator : MonoBehaviour
{
    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 0.2f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 0.01f;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 3f;

    private float distanciaPreviaPinch;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
        {
        #if UNITY_EDITOR

            // ───────────────── MOUSE EN EDITOR ─────────────────
            if (Input.GetMouseButton(0))
            {
                float rotY = Input.GetAxis("Mouse X") * 5f;
                transform.Rotate(Vector3.up, -rotY, Space.World);
            }

        #else

            // ───────────────── TOUCH EN CELULAR ─────────────────
            int cantidadToques = Touch.activeTouches.Count;

            if (cantidadToques == 1)
            {
                var touch = Touch.activeTouches[0];

                if (EventSystem.current != null &&
                    EventSystem.current.IsPointerOverGameObject(touch.touchId))
                    return;

                if (touch.delta.sqrMagnitude > 0)
                {
                    float rotY = touch.delta.x * rotationSpeed;
                    transform.Rotate(Vector3.up, -rotY, Space.World);
                }
            }

        #endif
        }

            public void AcercarModelo()
        {
            float nuevaEscala = Mathf.Clamp(
                transform.localScale.x + 0.1f,
                minScale,
                maxScale
            );
            transform.localScale = Vector3.one * nuevaEscala;
        }

        public void AlejarModelo()
        {
            float nuevaEscala = Mathf.Clamp(
                transform.localScale.x - 0.1f,
                minScale,
                maxScale
            );
            transform.localScale = Vector3.one * nuevaEscala;
        }

}