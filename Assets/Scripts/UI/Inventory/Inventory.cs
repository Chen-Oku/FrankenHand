using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEditor.Rendering;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;
    public GameObject inventoryUI; // Asigna el objeto de la UI del inventario en el Inspector
    private int allSlots;
    private int enabledSlots;
    public GameObject[] slots;
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
        Debug.Log("PlayerController encontrado: " + (playerController != null));
    }

        public bool IsInventoryUIActive()
    {
        return inventoryUI.activeSelf;
    }


    void Update()
    {
        // if (inventoryUI != null && IsInventoryUIActive()) //
        //     return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
            inventoryUI.SetActive(inventoryEnabled);

            if (inventoryEnabled)
            {
                // Si el menú de pausa está activo, ciérralo
                if (pauseMenu != null && pauseMenu.IsPauseMenuActive())
                    pauseMenu.Resume();

                TimeManager.Instance.RequestPause();
            }
            else
            {
                // Solo reanuda si no hay otro menú abierto
                if (pauseMenu == null || !pauseMenu.IsPauseMenuActive())
                {
                    TimeManager.Instance.RequestResume();
                }
            }
        }

        // Controla el PlayerController según si hay algún menú abierto
        if (playerController != null)
            playerController.enabled = !(inventoryEnabled || (pauseMenu != null && pauseMenu.IsPauseMenuActive()));
            

/*             inventoryEnabled = !inventoryEnabled;
                            inventoryUI.SetActive(inventoryEnabled);

                            if (inventoryEnabled)
                            {
                                if (pauseMenu != null && pauseMenu.IsPauseMenuActive())
                                    pauseMenu.Resume();

                                TimeManager.Instance.RequestPause();
                                if (playerController != null)
                                    playerController.enabled = false;
                            }
                            else
                            {
                                if (pauseMenu == null || !pauseMenu.IsPauseMenuActive())
                                {
                                    TimeManager.Instance.RequestResume();
                                    if (playerController != null)
                                        playerController.enabled = true;
                                }
                            } */
    
    }

    public bool AnyMenuOpen()
    {
        return (pauseMenu != null && pauseMenu.IsPauseMenuActive()) || inventoryEnabled;
    }


    public void CloseInventory()
    {
        inventoryEnabled = false;
        inventoryUI.SetActive(false);
        if (pauseMenu == null || !pauseMenu.IsPauseMenuActive())
        {
            TimeManager.Instance.RequestResume();
            if (playerController != null)
                playerController.enabled = true;
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
        // 1. Intenta poner el ítem en un slot vacío
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (slot != null && slot.empty)
            {
                slot.AddItem(pickup.itemData, pickup.amount);
                pickup.gameObject.SetActive(false);
                return;
            }
        }

        // 2. Si no hay slot vacío, intenta stackear en un slot existente
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (slot != null && !slot.empty && slot.itemSlot.itemData == pickup.itemData)
            {
                slot.itemSlot.quantity += pickup.amount;
                slot.UpdateSlot();
                pickup.gameObject.SetActive(false);
                return;
            }
        }

        // 3. Si no hay slot vacío ni stackeable, puedes mostrar mensaje de inventario lleno
        Debug.Log("Inventario lleno, no se pudo agregar el item.");
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (slot != null)
            {
                slot.selectedShader.SetActive(false);
                slot.thisItemSelected = false;
            }
        }
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



