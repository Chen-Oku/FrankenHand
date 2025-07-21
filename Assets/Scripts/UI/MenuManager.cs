using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuPausa menuPausa;
    public Inventory inventory;
    public ControladorOpciones opciones;
    public GameObject pantallaOpciones;
    public CanvasGroup opcionesCanvasGroup;

    private static MenuManager instance;
    public static MenuManager Instance => instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenPauseMenu()
    {
        CloseAllMenus();
        if (menuPausa != null)
            menuPausa.OpenPauseMenu();
    }

    public void OpenInventory()
    {
        CloseAllMenus();
        if (inventory != null)
            inventory.OpenInventory();
    }

    public void OpenOptionsMenu()
    {
        CloseAllMenus();
        if (opciones != null)
            opciones.SetOpcionesVisible(true);
    }

    public void CloseAllMenus()
    {
        if (menuPausa != null)
            menuPausa.ClosePauseMenu();
        if (inventory != null)
            inventory.CloseInventory();
        if (opciones != null)
            opciones.SetOpcionesVisible(false);
    }

    public bool AnyMenuOpen()
    {
        return (menuPausa != null && menuPausa.IsPauseMenuActive())
            || (inventory != null && inventory.IsInventoryUIActive())
            || (opciones != null && opciones.opcionesCanvasGroup != null && opciones.opcionesCanvasGroup.alpha > 0.5f);
    }

    void Update()
    {
        // Controla la pausa global
        Time.timeScale = AnyMenuOpen() ? 0f : 1f;

        // ESC: Toggle Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPausa != null && menuPausa.IsPauseMenuActive())
                menuPausa.ClosePauseMenu();
            else
                OpenPauseMenu();
        }

        // I: Toggle Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory != null && inventory.IsInventoryUIActive())
                inventory.CloseInventory();
            else
                OpenInventory();
        }
        // Puedes agregar aquí el input para abrir/cerrar opciones si lo deseas
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

    // Ejemplo al cerrar menú de pausa o inventario
    public void OnCloseMenu()
    {
        var playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
            playerController.puedeMover = true;
        }

        var playerInteraction = Object.FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = true;
    }

    public void UpdatePlayerControlState()
    {
        bool menusAbiertos = AnyMenuOpen();

        var playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
            playerController.enabled = !menusAbiertos;

        var playerInteraction = Object.FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.enabled = !menusAbiertos;
    }

    // Ejemplo desde MenuManager o cualquier menú
    public void ExampleMethodToOpenOptionsFromMenu()
    {
        CloseAllMenus();
        ControladorOpciones opciones = Object.FindFirstObjectByType<ControladorOpciones>();
        if (opciones != null)
        {
            opciones.previousMenu = gameObject; // El menú actual
            opciones.SetOpcionesVisible(true);
        }
    }
}
