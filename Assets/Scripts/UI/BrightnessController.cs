using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public Slider slider; // Reference to the UI Slider component
    public float sliderValue; // Variable to store the slider value
    public Image panelBrightness; // Reference to the UI Image component for brightness effect

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Brightness", 0.5f);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("Brightness", sliderValue);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, sliderValue);
    }
}