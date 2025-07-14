using UnityEngine;
using UnityEngine.UI;

public class VidaGUIProbeta : MonoBehaviour
{
    public VidaPlayer vidaPlayer;
    public Slider probetaSlider; // Asigna el Slider en el inspector

    void Start()
    {
        probetaSlider.maxValue = vidaPlayer.maxContenedores; // 9 medidas
        probetaSlider.wholeNumbers = true; // Solo valores enteros
        ActualizarProbeta();
        vidaPlayer.OnVidaCambiada += ActualizarProbeta;
    }

    public void ActualizarProbeta()
    {
        // Calcula la cantidad de "medidas" llenas
        int medidasLlenas = Mathf.CeilToInt((float)vidaPlayer.vidaActual / vidaPlayer.vidaPorContenedor);
        probetaSlider.value = medidasLlenas;
    }
}