using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private bool pauseMenuEnabled = false;

    void Start()
    {
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
        // No abrir el menú de pausa si el inventario está activo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuEnabled = !pauseMenuEnabled;
        }

        if (pauseMenuEnabled)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        pauseMenuEnabled = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
