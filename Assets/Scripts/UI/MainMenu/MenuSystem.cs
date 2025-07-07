using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        // Load the game scene
        SceneManager.LoadScene("Level1");
        Debug.Log("loading Level1");
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
