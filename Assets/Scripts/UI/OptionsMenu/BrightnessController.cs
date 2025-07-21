using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class brillo : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image panelBrillo;

    

    void Start()
    {
        if (slider == null)
            slider = GetComponentInChildren<Slider>();

        if (panelBrillo == null)
            panelBrillo = GameObject.Find("Panel Brightness")?.GetComponent<Image>();

        float savedBrightness = GameSettings.Instance != null ? GameSettings.Instance.Brightness : PlayerPrefs.GetFloat("Brightness", 1f);
        slider.value = savedBrightness;
        GameSettings.Instance.SetBrightness(savedBrightness);

        slider.onValueChanged.AddListener(ChangeSlider);
    }


    void Update()
    {

    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, slider.value);
    }

    public void ChangeBrightness(float value)
    {
        GameSettings.Instance.SetBrightness(value);
        slider.value = value;
    }
}