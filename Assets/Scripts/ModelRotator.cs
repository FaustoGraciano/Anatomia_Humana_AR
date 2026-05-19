using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ModelRotator : MonoBehaviour
{
    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 0.4f;

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
        int cantidadToques = Touch.activeTouches.Count;

        // ── Rotación — un dedo ───────────────────────────────────
        if (cantidadToques == 1)
        {
            var touch = Touch.activeTouches[0];

            // Ignorar si toca un botón
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(touch.touchId))
                return;

            if (touch.delta.sqrMagnitude > 0)
            {
                float rotY = touch.delta.x * rotationSpeed;
                // Solo rota en Y — gira sobre sí mismo
                transform.Rotate(Vector3.up, -rotY, Space.World);
            }
        }

        // ── Zoom — dos dedos (pinch) ─────────────────────────────
        if (cantidadToques == 2)
        {
            var touch0 = Touch.activeTouches[0];
            var touch1 = Touch.activeTouches[1];

            float distanciaActual = Vector2.Distance(
                touch0.screenPosition,
                touch1.screenPosition
            );

            // Primer frame del pinch — guardamos la distancia inicial
            if (touch0.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                touch1.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                distanciaPreviaPinch = distanciaActual;
                return;
            }

            float diferencia = distanciaActual - distanciaPreviaPinch;
            distanciaPreviaPinch = distanciaActual;

            // Aplicar zoom como escala uniforme
            float nuevaEscala = transform.localScale.x + diferencia * zoomSpeed;
            nuevaEscala = Mathf.Clamp(nuevaEscala, minScale, maxScale);
            transform.localScale = Vector3.one * nuevaEscala;
        }
    }
}