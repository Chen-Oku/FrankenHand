using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorOpciones : MonoBehaviour
{
    public GameObject pantallaOpciones;
    public GameObject previousMenu;
    public GameObject mainMenuUI;    // Asigna en el Inspector el panel del menú principal
    public GameObject pauseMenuUI;   // Asigna en el Inspector el panel del menú de pausa
    public CanvasGroup opcionesCanvasGroup; // Asigna en el Inspector o detecta automáticamente

    void Start()
    {
        // Detecta automáticamente el CanvasGroup si no está asignado
        if (opcionesCanvasGroup == null && pantallaOpciones != null)
            opcionesCanvasGroup = pantallaOpciones.GetComponentInChildren<CanvasGroup>();

        SetOpcionesVisible(false);
    }

    public void SetOpcionesVisible(bool visible)
    {
        if (pantallaOpciones != null && visible)
            pantallaOpciones.SetActive(true);

        if (opcionesCanvasGroup != null)
        {
            opcionesCanvasGroup.alpha = visible ? 1f : 0f;
            opcionesCanvasGroup.interactable = visible;
            opcionesCanvasGroup.blocksRaycasts = visible;
        }
        if (pantallaOpciones != null && !visible)
            pantallaOpciones.SetActive(false);
    }

    public void ReturnToPreviousMenu()
    {
        if (previousMenu != null)
        {
            previousMenu.SetActive(true);
        }
        else
        {
            string sceneName = SceneManager.GetActiveScene().name;

            // Buscar MainMenuUI automáticamente si no está asignado
            GameObject mainMenu = mainMenuUI;
            if (mainMenu == null)
            {
                mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
                if (mainMenu == null)
                    mainMenu = GameObject.Find("=== PauseMenu ==="); // Cambia el nombre si tu panel se llama diferente
            }

            // Buscar PauseMenuUI automáticamente si no está asignado
            GameObject pauseMenu = pauseMenuUI;
            if (pauseMenu == null)
            {
                pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
                if (pauseMenu == null)
                    pauseMenu = GameObject.Find("=== PauseMenu ==="); // Cambia el nombre si tu panel se llama diferente
            }

            if (sceneName == "MainMenu" && mainMenu != null)
            {
                mainMenu.SetActive(true);
            }
            else if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
        }

        SetOpcionesVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MostrarOpciones()
    {
        if (pantallaOpciones != null)
        {
            pantallaOpciones.SetActive(true); // Siempre antes
            SetOpcionesVisible(true);
        }
    }
}
