using System.Collections.Generic;
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

    void Start()
    {
        allSlots = slotHolder.transform.childCount;
        slots = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slots[i].GetComponent<Slot>().item == null)
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item"){
            GameObject itemPickedUp = other.gameObject;
            PickupItem pickup = itemPickedUp.GetComponent<PickupItem>();
            if (pickup != null && pickup.itemData != null)
            {
                AddItem(itemPickedUp, pickup.itemData, pickup.itemData.itemName, pickup.amount, pickup.itemData.icon);
                // Aquí puedes agregar lógica para marcar el objeto como recogido o destruirlo si es necesario
            }
        }
    }

    public void AddItem(GameObject itemObject, InventoryItemData itemData, string itemName, int quantity, Sprite icon)
    {
        // Busca si ya existe el item en el inventario (para stackear)
        InventoryItem existing = items.Find(i => i.itemData == itemData);
        if (existing != null)
        {
            existing.quantity += quantity;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData, itemName, icon, quantity);
            newItem.itemData = itemData;
            items.Add(newItem);
        }

        // Opcional: Actualiza la UI de slots aquí si lo necesitas

        // Desactiva o destruye el objeto recogido
        itemObject.SetActive(false);

        return;
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
    


