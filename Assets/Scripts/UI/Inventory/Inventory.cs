using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
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

    public List<InventoryItem> items = new List<InventoryItem>();
    public List<InventoryItem> collectibles = new List<InventoryItem>();
    public List<InventoryItem> keys = new List<InventoryItem>();


    public int maxSlots = 20;

    public PauseMenu pauseMenu; // Asigna el PauseMenu desde el Inspector

    void Start()
    {
        
        allSlots = slotHolder.transform.childCount;
        slots = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slots[i].GetComponent<Slot>().itemSlot == null)
            {
                slots[i].GetComponent<Slot>().empty = true; // Marca la ranura como vacía si no hay un item
            }
           
        }

        inventoryEnabled = false;
        inventoryUI.SetActive(inventoryEnabled); 

        playerController = Object.FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // No abrir el inventario si el menú de pausa está activo
        if (pauseMenu != null && pauseMenu.IsPauseMenuActive())
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
        }

        if (inventoryEnabled)
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PickupItem pickup))
        {
            AddItem(pickup);
        }
    }

    public void AddItem(PickupItem pickup)
    {
        // Busca si ya existe el item en el inventario (para stackear)
        InventoryItem existing = items.FirstOrDefault(i => i.itemData == pickup.itemData);

        if (existing != null)
        {
            existing.quantity += pickup.amount;
        }
        else
        {
            items.Add(new InventoryItem(pickup.itemData));
        }

        // Opcional: Actualiza la UI de slots aquí si lo necesitas
        UpdateSlot();

        // Desactiva o destruye el objeto recogido
        pickup.gameObject.SetActive(false);

        return;
    }

    public void UpdateSlot()
    {
        for (int i = 0; i < allSlots; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (i < items.Count)
            {
                slot.itemSlot = items[i];
                slot.empty = false;
            }
            else
            {
                slot.itemSlot = null;
                slot.empty = true;
            }
            slot.UpdateSlot();
        }
    }


    public bool RemoveItem(InventoryItemData itemData, int amount = 1)
    {
        // Implementa lógica similar para remover de la sección correcta
        return true;
    }

    // --- Guardado y carga ---
    [System.Serializable]
    public class InventorySaveData
    {
        public List<InventoryItem> items;
        public List<InventoryItem> collectibles;
        public List<InventoryItem> keys;
    }

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData
        {
            items = this.items,
            collectibles = this.collectibles,
            keys = this.keys
        };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("InventorySave", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        string json = PlayerPrefs.GetString("InventorySave", "");
        if (!string.IsNullOrEmpty(json))
        {
            InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
            this.items = data.items ?? new List<InventoryItem>();
            this.collectibles = data.collectibles ?? new List<InventoryItem>();
            this.keys = data.keys ?? new List<InventoryItem>();

            // Reasignar referencias a ScriptableObjects
            InventoryItemData[] allItems = Resources.LoadAll<InventoryItemData>("");
            foreach (var item in items)
                item.itemData = System.Array.Find(allItems, d => d.name == item.itemName);
            foreach (var item in collectibles)
                item.itemData = System.Array.Find(allItems, d => d.name == item.itemName);
            foreach (var item in keys)
                item.itemData = System.Array.Find(allItems, d => d.name == item.itemName);
        }
    }

}



