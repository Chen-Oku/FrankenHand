using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image imageMute;

    void Start()
    {
        // Initialize the slider value from PlayerPrefs or set a default value
        sliderValue = PlayerPrefs.GetFloat("Volume", 0.5f);
        AudioListener.volume = slider.value;
        CheckIfMuted();
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("Volume", sliderValue);
        AudioListener.volume = sliderValue;
        CheckIfMuted();
    }

    public void CheckIfMuted()
    {
        // Check if the volume is zero and update the mute icon accordingly
        if (slider.value == 0)
        {
            imageMute.enabled = true; // Show mute icon
        }
        else
        {
            imageMute.enabled = false; // Hide mute icon
        }
    }

}
