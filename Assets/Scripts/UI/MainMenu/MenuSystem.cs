using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuSystem : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public VideoPlayer introVideoPlayer; // Asigna en el inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        if (introVideoPlayer != null)
        {
            introVideoPlayer.gameObject.SetActive(true);
            introVideoPlayer.enabled = true;
            introVideoPlayer.Play();
            StartCoroutine(PlayAndLoadScene());
            mainMenuUI.SetActive(false); // Desactiva después de iniciar la coroutine
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }

    private System.Collections.IEnumerator PlayAndLoadScene()
    {
        // Espera a que termine el video
        while (introVideoPlayer.isPlaying)
            yield return null;

        SceneManager.LoadScene("Level1");
    }

    public void OpenOptionsFromMainMenu()
    {
        // Activa el objeto raíz del menú de opciones
        optionsMenuUI.SetActive(true); // Asegúrate de que el objeto raíz está activo

        var opcionesScript = optionsMenuUI.GetComponent<ControladorOpciones>();
        if (opcionesScript != null)
        {
            opcionesScript.previousMenu = mainMenuUI;
            opcionesScript.SetOpcionesVisible(true);
        }
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();

        // If running in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
