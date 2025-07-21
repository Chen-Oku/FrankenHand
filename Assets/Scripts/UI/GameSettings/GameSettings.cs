using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public float Volume { get; private set; }
    public float Brightness { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cargar valores guardados
        Volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        Brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        ApplyVolume();
        ApplyBrightness();
    }

    public void SetVolume(float value)
    {
        Volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        ApplyVolume();
    }

    public void SetBrightness(float value)
    {
        Brightness = value;
        PlayerPrefs.SetFloat("Brightness", value);
        ApplyBrightness();
    }

    private void ApplyVolume()
    {
        AudioListener.volume = Volume;
    }

    public void ApplyBrightness()
    {
        var overlay = GameObject.Find("Panel Brightness");
        if (overlay != null)
        {
            var img = overlay.GetComponent<UnityEngine.UI.Image>();
            if (img != null)
            {
                Color c = img.color;
                c.a = 0.5f - Brightness; // 0 = claro, 1 = oscuro
                img.color = c;
            }
        }
    }
}