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
            opcionesCanvasGroup = pantallaOpciones.GetComponent<CanvasGroup>();

        // Asegura que el menú de opciones inicie oculto y no interactivo
        SetOpcionesVisible(false);
    }

    /// <summary>
    /// Muestra el menú de opciones y asegura que el CanvasGroup esté correctamente configurado.
    /// </summary>
    public void MostrarOpciones()
    {
        // Detecta automáticamente el menú activo anterior si no está asignado
        if (previousMenu == null)
        {
            // Busca todos los GameObjects activos en la raíz del Canvas (excepto el propio menú de opciones)
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                foreach (Transform child in canvas.transform)
                {
                    if (child.gameObject != pantallaOpciones && child.gameObject.activeSelf)
                    {
                        previousMenu = child.gameObject;
                        break;
                    }
                }
            }
        }

        SetOpcionesVisible(true);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Activa o desactiva el menú de opciones y configura el CanvasGroup.
    /// </summary>
    public void SetOpcionesVisible(bool visible)
    {
        if (pantallaOpciones != null)
            pantallaOpciones.SetActive(visible);

        if (opcionesCanvasGroup != null)
        {
            opcionesCanvasGroup.alpha = visible ? 1f : 0f;
            opcionesCanvasGroup.interactable = visible;
            opcionesCanvasGroup.blocksRaycasts = visible;
        }
    }

    /// <summary>
    /// Vuelve al menú anterior o al menú principal/pausa si no hay uno definido.
    /// </summary>
    public void ReturnToPreviousMenu()
    {
        SetOpcionesVisible(false);

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
                mainMenu = GameObject.FindGameObjectWithTag("MainMenu");

            // Buscar PauseMenuUI automáticamente si no está asignado
            GameObject pauseMenu = pauseMenuUI;
            if (pauseMenu == null)
                pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");

            if (sceneName == "MainMenu" && mainMenu != null)
            {
                mainMenu.SetActive(true);
            }
            else if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
        }
    }
}
