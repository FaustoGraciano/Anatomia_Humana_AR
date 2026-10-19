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
    [SerializeField] private string nombreImagenQR = "images";

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
        if (trackedImageManager != null)
            trackedImageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);

        if (EnhancedTouchSupport.enabled)
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

            // Aparece 1.5 metros adelante de la cámara
            Camera arCamera = FindFirstObjectByType<Camera>();
            Vector3 posicion = arCamera.transform.position + 
                               arCamera.transform.forward * 1.5f;

            modelManager.IniciarEnPosicion(posicion);

            if (canvasUI != null)
                canvasUI.SetActive(true);
        }
    }
}


    // SOLO PARA TESTING EN EDITOR
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
