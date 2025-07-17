using UnityEngine;

public static class PlayerPreferencesManager
{
    public static void SetFullScreen(bool value)
    {
        PlayerPrefs.SetInt("pantallaCompleta", value ? 1 : 0);
    }

    public static bool GetFullScreen()
    {
        return PlayerPrefs.GetInt("pantallaCompleta", 1) == 1;
    }

    public static void SetResolution(int index)
    {
        PlayerPrefs.SetInt("numeroResolucion", index);
    }

    public static int GetResolution()
    {
        return PlayerPrefs.GetInt("numeroResolucion", 0);
    }
}