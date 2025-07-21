using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider slider;
    public Image imageMute;

    void Start()
    {
        slider.onValueChanged.AddListener(ChangeSlider);
        float savedVolume = GameSettings.Instance != null ? GameSettings.Instance.Volume : PlayerPrefs.GetFloat("Volume", 0.5f);
        slider.value = savedVolume;
        GameSettings.Instance.SetVolume(savedVolume); // Aplica el volumen al iniciar
        CheckIfMuted();
    }

    public void ChangeSlider(float value)
    {
        GameSettings.Instance.SetVolume(value);
        slider.value = value;
        CheckIfMuted();
    }

    public void CheckIfMuted()
    {
        imageMute.enabled = slider.value == 0;
    }
}
