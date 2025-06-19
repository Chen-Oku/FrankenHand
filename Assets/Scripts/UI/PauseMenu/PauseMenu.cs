using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    
    private bool pauseMenuEnabled = false;

    private PlayerController playerController; // Referencia al script de control del jugador

    public Inventory inventory; // Referencia al inventario, si es necesario

    void Start()
    {
        playerController = Object.FindFirstObjectByType<PlayerController>();

        pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public bool IsPauseMenuActive()
    {
        return pauseMenuUI.activeSelf;
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

        if (pauseMenuEnabled)
        {
            pauseMenuUI.SetActive(true);
            TimeManager.Instance.RequestPause();
            // Quita el control del player aquí
            // if (playerController != null)
            //     playerController.enabled = false;
        }
        else
        {
            pauseMenuUI.SetActive(false);
            TimeManager.Instance.RequestResume();
            // if (playerController != null)
            //     playerController.enabled = true;
        }
    }

    public bool AnyMenuOpen()
    {
        return IsPauseMenuActive() || (inventory != null && inventory.IsInventoryUIActive());
    }

    /*     void Update()
        {
            // No abrir el menú de pausa si el inventario está activo
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenuEnabled = !pauseMenuEnabled;

                if(pauseMenuEnabled && inventory != null && inventory.IsInventoryUIActive())
                {
                    inventory.CloseInventory(); // Cierra el inventario si está abierto
                }
            }

            if (pauseMenuEnabled)
            {
                pauseMenuUI.SetActive(true);
                TimeManager.Instance.RequestPause();
                if (playerController != null)
                        playerController.enabled = false;
            }
            else
            {
                pauseMenuUI.SetActive(false);
                TimeManager.Instance.RequestResume();
                if (playerController != null)
                            playerController.enabled = true;
            } 
        } */

    public void Resume()
    {
        pauseMenuEnabled = false;
        pauseMenuUI.SetActive(false);
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
            // Guarda quién abrió el menú de opciones
            var opcionesScript = optionsMenuUI.GetComponent<ControladorOpciones>();
            if (opcionesScript != null)
                opcionesScript.previousMenu = pauseMenuUI;

            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(true);
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
