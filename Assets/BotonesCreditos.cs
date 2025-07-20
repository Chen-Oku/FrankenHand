using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class BotonesCreditos : MonoBehaviour
{
    public GameObject botonesPanel; // Asigna el panel que contiene los botones en el inspector
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer en el inspector
    public float fadeDuration = 5f; // Duración del fade in

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (botonesPanel != null)
        {
            botonesPanel.SetActive(false); // Oculta los botones al inicio
            canvasGroup = botonesPanel.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoEnd; // Suscribirse al evento
    }

    public void MostrarBotones()
    {
        if (botonesPanel != null)
        {
            botonesPanel.SetActive(true);
            if (canvasGroup != null)
                StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambia por el nombre de tu escena de menú principal
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        MostrarBotones();
    }
}
