using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageTrackingManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private ModelManager modelManager;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private string nombreImagenQR = "QR Anatomia RA"; // debe coincidir con el nombre en la librería

    private bool modeloActivado = false;
    
public void SetCanvas(GameObject canvas)
{
    canvasUI = canvas;
}

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Solo nos interesa la primera vez que se detecta
        if (modeloActivado) return;

        foreach (ARTrackedImage trackedImage in args.added)
        {
            if (trackedImage.referenceImage.name == nombreImagenQR)
            {
                ActivarExperiencia(trackedImage.transform.position);
            }
        }
    }

    private void ActivarExperiencia(Vector3 posicion)
    {
        modeloActivado = true;

        // Posiciona y activa el cuerpo con piel en donde está el QR
        modelManager.IniciarEnPosicion(posicion);

        // Muestra los botones
        canvasUI.SetActive(true);
    }
}