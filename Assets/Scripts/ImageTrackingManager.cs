using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class ImageTrackingManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ModelManager modelManager;
    [SerializeField] private string nombreImagenQR = "qr_anatomia";

    private GameObject canvasUI;
    private bool qrDetectado = false;
    private bool modeloColocado = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public void SetCanvas(GameObject canvas)
    {
        canvasUI = canvas;
    }

    void OnEnable()
    {
        trackedImageManager.trackablesChanged.AddListener(OnTrackablesChanged);
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
        EnhancedTouchSupport.Disable();
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        if (qrDetectado) return;

        foreach (ARTrackedImage trackedImage in args.added)
        {
            if (trackedImage.referenceImage.name == nombreImagenQR)
            {
                qrDetectado = true;

                // Muestra los botones y un mensaje para que toque el suelo
                if (canvasUI != null)
                    canvasUI.SetActive(true);

                Debug.Log("QR detectado — tocá el suelo para colocar el modelo");
            }
        }
    }

    void Update()
    {
        // Solo escucha toques después de detectar el QR y antes de colocar el modelo
        if (!qrDetectado || modeloColocado) return;

        if (Touch.activeTouches.Count == 1)
        {
            var touch = Touch.activeTouches[0];

            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began) return;

            // Raycast contra planos reales detectados
            if (raycastManager.Raycast(touch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                modelManager.IniciarEnPosicion(hitPose.position);
                modeloColocado = true;
            }
        }
    }

    // SOLO PARA TESTING EN EDITOR — borrar antes de la build final
        void OnGUI()
{
        #if UNITY_EDITOR
        if (GUI.Button(new Rect(10, 10, 200, 50), "SIMULAR QR"))
        {
            qrDetectado = true;
            modelManager.IniciarEnPosicion(new Vector3(0, 0, 2));
            if (canvasUI != null)
                canvasUI.SetActive(true);
        }
        #endif
}
}
