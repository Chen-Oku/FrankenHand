using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    
    private bool pauseMenuEnabled = false;

    private PlayerController playerController; // Referencia al script de control del jugador

    public Inventory inventory; // Referencia al inventario, si es necesario

    private CanvasGroup pauseMenuUICanvasGroup; // Referencia al CanvasGroup del menú de pausa

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();

        // Quita el SetActive, solo usa CanvasGroup
        if (pauseMenuUI != null && pauseMenuUI.activeSelf)
            pauseMenuUI.SetActive(true); // Asegúrate de que esté activo para buscar el CanvasGroup

        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;

        pauseMenuEnabled = false;

        // Detecta automáticamente el CanvasGroup si no está asignado
        if (pauseMenuUICanvasGroup == null && pauseMenuUI != null)
            pauseMenuUICanvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();

        // Asegúrate de que el CanvasGroup esté desactivado al inicio
        if (pauseMenuUICanvasGroup != null)
            SetPauseMenuVisibility(false);
    }

    public void SetPauseMenuVisibility(bool visible)
    {
        if (pauseMenuUICanvasGroup != null)
        {
            pauseMenuUICanvasGroup.alpha = visible ? 1f : 0f;
            pauseMenuUICanvasGroup.interactable = visible;
            pauseMenuUICanvasGroup.blocksRaycasts = visible;
        }
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(visible); // Opcional, puedes quitarlo si solo usas CanvasGroup
    }

    public bool IsPauseMenuActive()
    {
        return pauseMenuUICanvasGroup != null ? pauseMenuUICanvasGroup.alpha > 0.5f : pauseMenuUI.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuEnabled = !pauseMenuEnabled;

            if (pauseMenuEnabled && inventory != null && inventory.IsInventoryUIActive())
            {
                inventory.CloseInventory(); // Cierra el inventario si está abierto
            }
        }

        SetPauseMenuVisibility(pauseMenuEnabled);

        if (pauseMenuEnabled)
        {
            TimeManager.Instance.RequestPause();
            // if (playerController != null)
            //     playerController.enabled = false;
        }
        else
        {
            TimeManager.Instance.RequestResume();
            // if (playerController != null)
            //     playerController.enabled = true;
        }
    }

    public bool AnyMenuOpen()
    {
        return IsPauseMenuActive() || (inventory != null && inventory.IsInventoryUIActive());
    }

    public void Resume()
    {
        pauseMenuEnabled = false;
        SetPauseMenuVisibility(false);
        TimeManager.Instance.RequestResume();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenOptions()
    {
        if (optionsMenuUI != null)
        {
            // Activa el objeto raíz del menú de opciones
            optionsMenuUI.SetActive(true); // Asegúrate de que el objeto raíz está activo

            var opcionesScript = optionsMenuUI.GetComponent<ControladorOpciones>();
            if (opcionesScript != null)
            {
                opcionesScript.previousMenu = pauseMenuUI;
                opcionesScript.SetOpcionesVisible(true);
            }
            SetPauseMenuVisibility(false); // Oculta el menú de pausa usando CanvasGroup
        }
    }

    public void CloseOptions()
    {
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false); // Oculta el menú de opciones
        pauseMenuUI.SetActive(true); // Muestra el menú de pausa
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Asegúrate que la escena se llame "MainMenu" o cambia el nombre aquí
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Salir del juego.");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // If running in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // Quit the application
        Application.Quit();
        
        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
