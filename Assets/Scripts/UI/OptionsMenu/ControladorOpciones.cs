using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorOpciones : MonoBehaviour
{
    public GameObject pantallaOpciones;
    public GameObject previousMenu;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public CanvasGroup opcionesCanvasGroup;

    void Start()
    {
        if (opcionesCanvasGroup == null && pantallaOpciones != null)
            opcionesCanvasGroup = pantallaOpciones.GetComponent<CanvasGroup>();
        SetOpcionesVisible(false);
    }

    public void MostrarOpciones()
    {
        if (previousMenu == null)
        {
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
            GameObject mainMenu = mainMenuUI;
            if (mainMenu == null)
                mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
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
