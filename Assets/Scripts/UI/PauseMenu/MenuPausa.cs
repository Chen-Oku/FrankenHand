using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausaUI;
    public CanvasGroup menuPausaCanvasGroup; // Asigna el CanvasGroup del menú de pausa
    public Button resumeButton;
    public Button restartButton;
    public Button optionsButton;
    public Button mainMenuButton;
    public Button quitButton;
    public Button openPauseMenuGUIButton;

    private bool isPaused = false;
    private Inventory inventory;
    private ControladorOpciones opcionesScript;
    private PlayerController player;
    private PlayerSoundController playerSound;

    void Start()
    {
        if (menuPausaCanvasGroup == null && menuPausaUI != null)
            menuPausaCanvasGroup = menuPausaUI.GetComponent<CanvasGroup>();

        SetPauseMenuVisible(false);

        inventory = Object.FindFirstObjectByType<Inventory>();
        opcionesScript = Object.FindFirstObjectByType<ControladorOpciones>();
        player = Object.FindFirstObjectByType<PlayerController>();
        playerSound = Object.FindFirstObjectByType<PlayerSoundController>();

        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (restartButton != null) restartButton.onClick.AddListener(RestartLevel);
        if (optionsButton != null) optionsButton.onClick.AddListener(OpenOptions);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (openPauseMenuGUIButton != null) openPauseMenuGUIButton.onClick.AddListener(TogglePauseMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (inventory != null && inventory.IsInventoryUIActive())
            inventory.CloseInventory();

        if (opcionesScript != null && opcionesScript.opcionesCanvasGroup != null && opcionesScript.opcionesCanvasGroup.alpha > 0.5f)
            opcionesScript.SetOpcionesVisible(false);

        SetPauseMenuVisible(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (player != null)
            player.enabled = false;
        if (playerSound != null && playerSound.audioSource != null)
            playerSound.audioSource.enabled = false;

        var playerInteraction = Object.FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = false;
    }

    public void OpenPauseMenu()
    {
        SetPauseMenuVisible(true);
        isPaused = true;
        MenuManager.Instance.UpdatePlayerControlState();
    }

    public void ClosePauseMenu()
    {
        SetPauseMenuVisible(false);
        isPaused = false;
        MenuManager.Instance.UpdatePlayerControlState();
    }

    public void Resume()
    {
        SetPauseMenuVisible(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (player != null)
            player.enabled = true;
        if (playerSound != null && playerSound.audioSource != null)
            playerSound.audioSource.enabled = true;

        var playerInteraction = Object.FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;

        var playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.puedeMover = true;

        TimeManager.Instance.RequestResume();
    }

    private void SetPauseMenuVisible(bool visible)
    {
        if (menuPausaCanvasGroup != null)
        {
            menuPausaCanvasGroup.alpha = visible ? 1f : 0f;
            menuPausaCanvasGroup.interactable = visible;
            menuPausaCanvasGroup.blocksRaycasts = visible;
        }
        if (menuPausaUI != null)
            menuPausaUI.SetActive(visible);
    }

    public void RestartLevel()
    {
        Resume();


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenOptions()
    {
        SetPauseMenuVisible(false);
        if (opcionesScript != null)
        {
            opcionesScript.previousMenu = menuPausaUI;
            opcionesScript.SetOpcionesVisible(true);
        }
    }

    public void GoToMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Permite a otros scripts saber si el menú está activo
    public bool IsPauseMenuActive()
    {
        return menuPausaCanvasGroup != null ? menuPausaCanvasGroup.alpha > 0.5f : (menuPausaUI != null && menuPausaUI.activeSelf);
    }
}
