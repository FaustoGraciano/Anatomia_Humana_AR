using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class HotspotManager : MonoBehaviour
{
    [Header("Panel de información (se crea automático)")]
    private GameObject panelInfo;
    private Text textoNombre;
    private Text textoDescripcion;
    private bool panelVisible = false;

    void OnEnable()  => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
    #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.Log("CLICK EN: " + hit.collider.name);

                Hotspot hotspot = hit.collider.GetComponent<Hotspot>();

                if (hotspot != null)
                {
                    MostrarPanel(hotspot.nombreEstructura, hotspot.descripcion);
                }
            }
        }
    #else
        if (Touch.activeTouches.Count == 1)
        {
            var touch = Touch.activeTouches[0];

            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began)
                return;

            Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Hotspot hotspot = hit.collider.GetComponent<Hotspot>();

                if (hotspot != null)
                {
                    MostrarPanel(hotspot.nombreEstructura, hotspot.descripcion);
                }
            }
        }
    #endif
    }

    private void MostrarPanel(string nombre, string descripcion)
    {
        if (textoNombre == null || textoDescripcion == null || panelInfo == null)
        {
            Debug.LogWarning("El panel de información aún no se ha inicializado con el Canvas.");
            return;
        }

        textoNombre.text = nombre;
        textoDescripcion.text = descripcion;
        panelInfo.SetActive(true);
        panelVisible = true;
    }

    public void OcultarPanel()
    {
        if (panelInfo != null)
        {
            panelInfo.SetActive(false);
            panelVisible = false;
        }
    }

    public void InicializarPanelConCanvas(Canvas canvas)
    {
        if (canvas == null) return;

        // 1. Fondo negro del Panel (Altura dinámica heredada)
        panelInfo = new GameObject("PanelHotspot");
        panelInfo.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panelInfo.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(600, 220); // Tamaño inicial base

        Image panelImg = panelInfo.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.82f);

        // Diseñador vertical automático para el Panel completo (Mantiene tu autodimensión)
        VerticalLayoutGroup layout = panelInfo.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(24, 45, 24, 24); // Margen derecho extra para que el texto no pise la X
        layout.spacing = 12; 
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        ContentSizeFitter panelFitter = panelInfo.AddComponent<ContentSizeFitter>();
        panelFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        panelFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        // ── BOTÓN DE CIERRE (X) CON EL TRUCO PARA IGNORES EL LAYOUT ──
        GameObject goCierre = new GameObject("BotonCerrarHotspot");
        goCierre.transform.SetParent(panelInfo.transform, false);
        
        RectTransform rCierre = goCierre.AddComponent<RectTransform>();
        rCierre.anchorMin = new Vector2(1, 1); // Anclado arriba a la derecha
        rCierre.anchorMax = new Vector2(1, 1);
        rCierre.pivot = new Vector2(1, 1);     // Pivote arriba a la derecha
        rCierre.anchoredPosition = new Vector2(-12, -12); // Clavado justo en el borde interno superior derecho
        rCierre.sizeDelta = new Vector2(35, 35);

        // MÁGIA: Esto hace que ignore por completo el VerticalLayoutGroup y se quede fijo en la esquina
        LayoutElement layoutElement = goCierre.AddComponent<LayoutElement>();
        layoutElement.ignoreLayout = true;

        Button btnCierre = goCierre.AddComponent<Button>();
        btnCierre.onClick.AddListener(OcultarPanel);

        GameObject goCierreTexto = new GameObject("TextoX");
        goCierreTexto.transform.SetParent(goCierre.transform, false);
        RectTransform rCierreTexto = goCierreTexto.AddComponent<RectTransform>();
        rCierreTexto.anchorMin = Vector2.zero;
        rCierreTexto.anchorMax = Vector2.one;
        rCierreTexto.offsetMin = Vector2.zero;
        rCierreTexto.offsetMax = Vector2.zero;

        Text txtCierre = goCierreTexto.AddComponent<Text>();
        txtCierre.text = "✕";
        txtCierre.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txtCierre.fontSize = 24;
        txtCierre.fontStyle = FontStyle.Bold;
        txtCierre.alignment = TextAnchor.MiddleCenter;
        txtCierre.color = new Color(0.9f, 0.25f, 0.25f, 1f);

        // 2. Componente de Texto del Nombre
        GameObject goNombre = new GameObject("TextoNombre");
        goNombre.transform.SetParent(panelInfo.transform, false);
        
        textoNombre = goNombre.AddComponent<Text>();
        textoNombre.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textoNombre.fontSize = 24;
        textoNombre.fontStyle = FontStyle.Bold;
        textoNombre.color = Color.white;
        textoNombre.text = "";
        textoNombre.horizontalOverflow = HorizontalWrapMode.Wrap;
        textoNombre.verticalOverflow = Vector2.zero == Vector2.zero ? VerticalWrapMode.Overflow : VerticalWrapMode.Truncate;

        // 3. Componente de Texto de la Descripción
        GameObject goDesc = new GameObject("TextoDescripcion");
        goDesc.transform.SetParent(panelInfo.transform, false);

        textoDescripcion = goDesc.AddComponent<Text>();
        textoDescripcion.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textoDescripcion.fontSize = 18;
        textoDescripcion.color = new Color(0.85f, 0.85f, 0.85f, 1f);
        textoDescripcion.text = "";
        textoDescripcion.horizontalOverflow = HorizontalWrapMode.Wrap;
        textoDescripcion.verticalOverflow = VerticalWrapMode.Overflow;

        panelInfo.SetActive(false);
    }
}