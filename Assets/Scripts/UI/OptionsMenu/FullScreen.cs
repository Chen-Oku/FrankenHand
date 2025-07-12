/* using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        CheckResolutions();
    }

    public void ActivarFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

     public void CheckResolutions()
    {
        resolutions = Screen.resolutions;
        resolitionsDropdown.ClearOptions();
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

        resolutionDropdown.value = PlayerPrefs.GetInt("numeroResoluciones", 0);
    }


    public void ChangeResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("numeroResoluciones", resolutionDropdown.value);

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
 */

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
using TMPro;
//

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;

    //
    public TMP_Dropdown resolucionesDropDown;
    Resolution[] resoluciones;
    //

    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        //
        RevisarResolucion();
        //
    }


    void Update()
    {

    }

    public void ActiveFULLS(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;

    }

    //
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropDown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);


            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }

        }

        resolucionesDropDown.AddOptions(opciones);
        resolucionesDropDown.value = resolucionActual;
        resolucionesDropDown.RefreshShownValue();


        //
        resolucionesDropDown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
        //
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        //
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropDown.value);
        //


        Resolution resolution = resoluciones[indiceResolucion];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    //
}