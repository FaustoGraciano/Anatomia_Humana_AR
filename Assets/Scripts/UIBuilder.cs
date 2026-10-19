using UnityEngine;
using UnityEngine.UI;

public class UIBuilder : MonoBehaviour
{
    [SerializeField] private ModelManager modelManager;

    private GameObject canvasGO;

    void Awake()
    {
        CrearCanvas();
    }

    private void CrearCanvas()
    {
        // ── Canvas ──────────────────────────────────────────────
        canvasGO = new GameObject("CanvasUI");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        // EventSystem (necesario para que los botones respondan al touch)
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // ── Panel contenedor (arriba a la izquierda) ─────────────
        GameObject panel = new GameObject("PanelBotones");
        panel.transform.SetParent(canvasGO.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 1);
        panelRect.anchorMax = new Vector2(0, 1);
        panelRect.pivot = new Vector2(0, 1);
        panelRect.anchoredPosition = new Vector2(30, -30);
        panelRect.sizeDelta = new Vector2(200, 280);

        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.45f);
        panelImg.raycastTarget = false; // solo el panel, los botones sí reciben raycast

        // Bordes redondeados simulados con padding usando VerticalLayoutGroup
        VerticalLayoutGroup layout = panel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(14, 14, 14, 14);
        layout.spacing = 12;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        // ── Botones ──────────────────────────────────────────────
        CrearBoton(panel, "Cuerpo",    new Color(0.20f, 0.60f, 0.86f, 1f), () => modelManager.OnClickCuerpo());
        CrearBoton(panel, "Huesos",    new Color(0.94f, 0.94f, 0.94f, 1f), () => modelManager.OnClickHuesos());
        CrearBoton(panel, "Órganos",   new Color(0.85f, 0.33f, 0.31f, 1f), () => modelManager.OnClickOrganos());
        CrearBoton(panel, "Músculos",  new Color(0.95f, 0.61f, 0.07f, 1f), () => modelManager.OnClickMusculos());

        // Separador visual
        GameObject separador = new GameObject("Separador");
        separador.transform.SetParent(panel.transform, false);
        RectTransform sepRect = separador.AddComponent<RectTransform>();
        sepRect.sizeDelta = new Vector2(0, 2);
        Image sepImg = separador.AddComponent<Image>();
        sepImg.color = new Color(1f, 1f, 1f, 0.2f);
        sepImg.raycastTarget = false;

        // Botones de zoom
        ModelRotator rotator = FindFirstObjectByType<ModelRotator>();
        CrearBoton(panel, "＋  Acercar", new Color(0.2f, 0.75f, 0.4f, 1f), () => rotator?.AcercarModelo());
        CrearBoton(panel, "－  Alejar",  new Color(0.2f, 0.75f, 0.4f, 1f), () => rotator?.AlejarModelo());


         HotspotManager hotspotManager = FindFirstObjectByType<HotspotManager>();
        if (hotspotManager != null)
        {
            hotspotManager.InicializarPanelConCanvas(canvas);
        }

        // Empieza oculto — se activa cuando se detecta el QR
        canvasGO.SetActive(false);

        // Exponemos la referencia al ImageTrackingManager
        ImageTrackingManager tracker = FindObjectOfType<ImageTrackingManager>();
        if (tracker != null)
            tracker.SetCanvas(canvasGO);

    }

    private void CrearBoton(GameObject parent, string label, Color color, UnityEngine.Events.UnityAction accion)
    {
        GameObject btnGO = new GameObject("Btn_" + label);
        btnGO.transform.SetParent(parent.transform, false);

        RectTransform rect = btnGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, 52);

        Image img = btnGO.AddComponent<Image>();
        img.color = color;

        Button btn = btnGO.AddComponent<Button>();

        // Efecto hover/press
        ColorBlock cb = btn.colors;
        cb.normalColor = color;
        cb.highlightedColor = color * 1.15f;
        cb.pressedColor = color * 0.75f;
        cb.selectedColor = color;
        btn.colors = cb;

        btn.onClick.AddListener(accion);

        // Texto del botón
        GameObject textGO = new GameObject("Texto");
        textGO.transform.SetParent(btnGO.transform, false);

        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text texto = textGO.AddComponent<Text>();
        texto.text = label;
        texto.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        texto.fontSize = 22;
        texto.fontStyle = FontStyle.Bold;
        texto.alignment = TextAnchor.MiddleCenter;

        // Color de texto según luminosidad del fondo
        float luminosidad = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
        texto.color = luminosidad > 0.6f ? new Color(0.1f, 0.1f, 0.1f) : Color.white;
    }
}