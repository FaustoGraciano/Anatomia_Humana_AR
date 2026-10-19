using UnityEngine;

public class Hotspot : MonoBehaviour
{
    [Header("Información")]
    public string nombreEstructura = "Fémur";
    
    [TextArea(3, 6)]
    public string descripcion = "Función: Soporta el peso del cuerpo en el muslo y conecta la articulación de la cadera con la rodilla.\n\nCuriosidades: Es el hueso más largo, pesado y resistente del cuerpo humano. Puede soportar impactos de hasta 30 veces tu propio peso. En su interior (la médula ósea) se producen la mayoría de tus glóbulos rojos.";
}
