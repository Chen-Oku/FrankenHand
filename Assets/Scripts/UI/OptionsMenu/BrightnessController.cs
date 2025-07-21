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

        // Inicializa el valor del slider y el panel
        float savedBrightness = GameSettings.Instance != null ? GameSettings.Instance.Brightness : PlayerPrefs.GetFloat("Brightness", 1f);
        slider.value = savedBrightness;
        GameSettings.Instance.SetBrightness(savedBrightness); // Aplica el brillo al iniciar

        slider.onValueChanged.AddListener(ChangeSlider);
    }

    //Update is called once per frame
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