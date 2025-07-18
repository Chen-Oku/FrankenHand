using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResolutionScreen : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        CheckResolutions();

        // Buscar 1920x1080 o la más cercana
        int defaultWidth = 1920;
        int defaultHeight = 1080;
        int closestIndex = 0;
        int minDiff = int.MaxValue;

        for (int i = 0; i < resolutions.Length; i++)
        {
            int diff = Mathf.Abs(resolutions[i].width - defaultWidth) + Mathf.Abs(resolutions[i].height - defaultHeight);
            if (diff < minDiff)
            {
                minDiff = diff;
                closestIndex = i;
            }
            // Si es exactamente 1920x1080, selecciona y termina
            if (resolutions[i].width == defaultWidth && resolutions[i].height == defaultHeight)
            {
                closestIndex = i;
                break;
            }
        }

        int savedResolutionIndex = PlayerPrefs.GetInt("numeroResolucion", closestIndex);
        SetResolution(savedResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void CheckResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("numeroResolucion", resolutionIndex);
    }

    private void OnEnable()
    {
        int savedResolutionIndex = PlayerPrefs.GetInt("numeroResolucion", 0);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
}
