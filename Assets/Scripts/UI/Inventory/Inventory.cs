using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;
    public GameObject inventoryUI; // Asigna el objeto de la UI del inventario en el Inspector
    private int allSlots;
    private int enabledSlots;
    private GameObject[] slots;
    public GameObject slotHolder; // Referencia al objeto contenedor de los slots

    private PlayerController playerController; // Referencia al script de control del jugador

    void Start()
    {
        allSlots = slotHolder.transform.childCount;
        slots = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        inventoryEnabled = false;
        inventoryUI.SetActive(inventoryEnabled); 

        playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
        }
        if (inventoryEnabled == true)
        {
            inventoryUI.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
            if (playerController != null)
                playerController.enabled = false; // Desactiva el control del jugador
        }
        else
        {
            inventoryUI.SetActive(false);
            Time.timeScale = 1f; // Reanuda el juego
            if (playerController != null)
                playerController.enabled = true; // Reactiva el control del jugador
        }
    }
}
