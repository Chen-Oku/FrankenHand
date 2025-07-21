using UnityEngine;
using UnityEngine.UI;

public class BrightnessPanelUpdater : MonoBehaviour
{
    public Slider slider; // Referencia al componente Slider en el panel de brillo

    void Start()
    {
        if (GameSettings.Instance != null)
            GameSettings.Instance.ApplyBrightness();
    }

    public void ChangeBrightness(float value)
    {
        GameSettings.Instance.SetBrightness(value);
        slider.value = value;
    }
}