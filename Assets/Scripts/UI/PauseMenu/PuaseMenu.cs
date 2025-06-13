using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private bool pauseMenuEnabled = false;

    private PlayerController playerController; // Referencia al script de control del jugador

    void Start()
    {
        pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuEnabled = !pauseMenuEnabled;
        }

        if (pauseMenuEnabled)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            if (playerController != null)
                playerController.enabled = false; // Desactiva el control del jugador
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            if (playerController != null)
                playerController.enabled = true; // Reactiva el control del jugador
        }
    }

    public void Resume()
    {
        pauseMenuEnabled = false;
    }

    public void Pause()
    {
        pauseMenuEnabled = true;
    }

    public void SaveGame()
    {
        // Implementa tu lógica de guardado aquí
        Debug.Log("Juego guardado.");
    }

    public void LoadGame()
    {
        // Implementa tu lógica de carga aquí
        Debug.Log("Juego cargado.");
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego.");
    }
}
