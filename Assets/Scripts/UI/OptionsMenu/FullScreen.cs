using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        // Recupera la preferencia de pantalla completa
        bool pantallaCompleta = PlayerPrefs.GetInt("pantallaCompleta", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = pantallaCompleta;
        toggle.isOn = pantallaCompleta;
    }

    public void CambiarFullScreen(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        PlayerPrefs.SetInt("pantallaCompleta", pantallaCompleta ? 1 : 0);
    }
}