using UnityEngine;

public class PaneoEventRelay : MonoBehaviour
{
    public PaneoTrigger paneoTrigger;

    // Llama este método desde un Animation Event para desactivar objetos
    public void DesactivarObjetos()
    {
        if (paneoTrigger != null)
            paneoTrigger.DesactivarObjetos();
    }

    // Llama este método desde un Animation Event al final del paneo
    public void OnPaneoTerminado()
    {
        if (paneoTrigger != null)
            paneoTrigger.OnPaneoTerminado();
    }
}