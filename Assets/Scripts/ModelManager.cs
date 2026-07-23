using UnityEngine;

public class ModelManager : MonoBehaviour
{
    [Header("Modelos")]
    [SerializeField] private GameObject modeloCuerpo;
    [SerializeField] private GameObject modeloHuesos;
    [SerializeField] private GameObject modeloOrganos;
    [SerializeField] private GameObject modeloSistemas;    // ← nuevo
    [SerializeField] private GameObject modeloMusculos;

    private GameObject modeloActivo;
    private GameObject modeloActivoSecundario;    // ← para el segundo modelo simultáneo

    void Awake()
    {
        modeloCuerpo.SetActive(false);
        modeloHuesos.SetActive(false);
        modeloOrganos.SetActive(false);
        modeloSistemas.SetActive(false);    // ← nuevo
        modeloMusculos.SetActive(false);
    }

    public void IniciarEnPosicion(Vector3 posicion)
    {
        transform.position = posicion;
        transform.localScale = Vector3.one * 0.1f;
        MostrarModelo(modeloCuerpo);
    }

    public void OnClickCuerpo()   => MostrarModelo(modeloCuerpo);
    public void OnClickHuesos()   => MostrarModelo(modeloHuesos);
    public void OnClickMusculos() => MostrarModelo(modeloMusculos);

    public void OnClickOrganos()
    {
        // Desactiva lo que haya activo
        if (modeloActivo != null)
            modeloActivo.SetActive(false);

        if (modeloActivoSecundario != null)
            modeloActivoSecundario.SetActive(false);

        // Activa los dos modelos juntos
        modeloOrganos.SetActive(true);
        modeloSistemas.SetActive(true);

        modeloActivo = modeloOrganos;
        modeloActivoSecundario = modeloSistemas;
    }

    private void MostrarModelo(GameObject nuevo)
    {
        if (modeloActivo != null)
            modeloActivo.SetActive(false);

        // Desactiva el secundario si había uno activo (ej: venía de órganos)
        if (modeloActivoSecundario != null)
        {
            modeloActivoSecundario.SetActive(false);
            modeloActivoSecundario = null;
        }

        nuevo.SetActive(true);
        modeloActivo = nuevo;
    }
}