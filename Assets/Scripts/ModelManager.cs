using UnityEngine;

public class ModelManager : MonoBehaviour
{
    [Header("Modelos")]
    [SerializeField] private GameObject modeloCuerpo;    // con piel
    [SerializeField] private GameObject modeloHuesos;
    [SerializeField] private GameObject modeloOrganos;
    [SerializeField] private GameObject modeloMusculos;

    private GameObject modeloActivo;

    void Awake()
    {
        // Todos desactivados al arrancar
        modeloCuerpo.SetActive(false);
        modeloHuesos.SetActive(false);
        modeloOrganos.SetActive(false);
        modeloMusculos.SetActive(false);
    }

    // Llamado por ImageTrackingManager cuando se detecta el QR
    public void IniciarEnPosicion(Vector3 posicion)
    {
        transform.position = posicion;
        MostrarModelo(modeloCuerpo);
    }

    // Botones
    public void OnClickCuerpo()   => MostrarModelo(modeloCuerpo);
    public void OnClickHuesos()   => MostrarModelo(modeloHuesos);
    public void OnClickOrganos()  => MostrarModelo(modeloOrganos);
    public void OnClickMusculos() => MostrarModelo(modeloMusculos);

    private void MostrarModelo(GameObject nuevo)
    {
        if (modeloActivo != null)
            modeloActivo.SetActive(false);

        nuevo.SetActive(true);
        modeloActivo = nuevo;
    }
}