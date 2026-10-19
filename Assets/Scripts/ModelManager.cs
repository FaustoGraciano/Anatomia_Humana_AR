using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ModelManager : MonoBehaviour
{
    [Header("Modelos")]
    [SerializeField] private GameObject modeloCuerpo;
    [SerializeField] private GameObject modeloHuesos;
    [SerializeField] private GameObject modeloOrganos;
    [SerializeField] private GameObject modeloSistemas;
    [SerializeField] private GameObject modeloMusculos;

    private GameObject modeloActivo;
    private GameObject modeloActivoSecundario;
    private ARAnchor ancla;

    void Awake()
    {
        modeloCuerpo.SetActive(false);
        modeloHuesos.SetActive(false);
        modeloOrganos.SetActive(false);
        modeloSistemas.SetActive(false);
        modeloMusculos.SetActive(false);
    }

    public void IniciarEnPosicion(Vector3 posicion)
    {
        // Crea un ancla en esa posiciÃ³n para fijar el modelo al mundo real
        GameObject anclaGO = new GameObject("ModeloAncla");
        anclaGO.transform.position = posicion;
        ancla = anclaGO.AddComponent<ARAnchor>();

        // El modelo pasa a ser hijo del ancla
        transform.SetParent(ancla.transform);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * 0.1f;

        MostrarModelo(modeloCuerpo);
    }

    public void OnClickCuerpo()   => MostrarModelo(modeloCuerpo);
    public void OnClickHuesos()   => MostrarModelo(modeloHuesos);
    public void OnClickMusculos() => MostrarModelo(modeloMusculos);

    public void OnClickOrganos()
    {
        if (modeloActivo != null)
            modeloActivo.SetActive(false);
        if (modeloActivoSecundario != null)
            modeloActivoSecundario.SetActive(false);

        modeloOrganos.SetActive(true);
        modeloSistemas.SetActive(true);

        modeloActivo = modeloOrganos;
        modeloActivoSecundario = modeloSistemas;
    }

    private void MostrarModelo(GameObject nuevo)
    {
        if (modeloActivo != null)
            modeloActivo.SetActive(false);

        if (modeloActivoSecundario != null)
        {
            modeloActivoSecundario.SetActive(false);
            modeloActivoSecundario = null;
        }

        nuevo.SetActive(true);
        modeloActivo = nuevo;
    }
}