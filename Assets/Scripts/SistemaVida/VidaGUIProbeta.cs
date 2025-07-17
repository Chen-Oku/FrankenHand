using UnityEngine;
using UnityEngine.UI;

public class VidaGUIProbeta : MonoBehaviour
{
    public VidaPlayer vidaPlayer;
    public Slider probetaSlider;

    void Start()
    {
        if (vidaPlayer == null)
            vidaPlayer = Object.FindFirstObjectByType<VidaPlayer>();

        if (vidaPlayer != null)
            vidaPlayer.OnVidaCambiada += ActualizarProbeta;

        ActualizarProbeta();
    }

    public void ActualizarProbeta()
    {
        if (probetaSlider == null || vidaPlayer == null)
            return;

        probetaSlider.maxValue = vidaPlayer.maxContenedores * vidaPlayer.vidaPorContenedor;
        probetaSlider.value = vidaPlayer.vidaActual;
    }

    void OnDestroy()
    {
        if (vidaPlayer != null)
            vidaPlayer.OnVidaCambiada -= ActualizarProbeta;
    }
}