using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;
    public GameObject inventoryUI; // Asigna el objeto de la UI del inventario
    private int allSlots;
    private int enabledSlots;
    public GameObject[] slots;
    public GameObject slotHolder; // Referencia al objeto contenedor de los slots

    private PlayerController playerController; // Referencia al script de control del jugador

    public List<InventoryItem> others = new List<InventoryItem>();
    public List<InventoryItem> usable = new List<InventoryItem>();
    public List<InventoryItem> collectibles = new List<InventoryItem>();
    public List<InventoryItem> keys = new List<InventoryItem>();


    public int maxSlots = 20;

    public PauseMenu pauseMenu; // Asigna el PauseMenu desde el Inspector
    public CanvasGroup inventoryCanvasGroup; // Asigna el CanvasGroup desde el Inspector

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

        // Detecta automáticamente el CanvasGroup si no está asignado
        if (inventoryCanvasGroup == null && inventoryUI != null)
            inventoryCanvasGroup = inventoryUI.GetComponent<CanvasGroup>();

        // Asegúrate de que el CanvasGroup esté desactivado al inicio
        if (inventoryCanvasGroup != null)
            SetInventoryUIVisible(false);

        playerController = Object.FindFirstObjectByType<PlayerController>();
        Debug.Log("PlayerController encontrado: " + (playerController != null));
    }

    // Añade este método para controlar la visibilidad e interacción del inventario
    private void SetInventoryUIVisible(bool visible)
    {
        if (inventoryCanvasGroup != null)
        {
            inventoryCanvasGroup.alpha = visible ? 1f : 0f;
            inventoryCanvasGroup.interactable = visible;
            inventoryCanvasGroup.blocksRaycasts = visible;
        }
        if (inventoryUI != null)
            inventoryUI.SetActive(visible); // Opcional, si quieres seguir usando SetActive
    }

    public bool IsInventoryUIActive()
    {
        // Si usas solo CanvasGroup, puedes comprobar el alpha o interactable
        return inventoryCanvasGroup != null ? inventoryCanvasGroup.alpha > 0.5f : inventoryUI.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
            SetInventoryUIVisible(inventoryEnabled);

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
    }

    public bool AnyMenuOpen()
    {
        return (pauseMenu != null && pauseMenu.IsPauseMenuActive()) || inventoryEnabled;
    }


    public void CloseInventory()
    {
        inventoryEnabled = false;
        SetInventoryUIVisible(false);
        if (pauseMenu == null || !pauseMenu.IsPauseMenuActive())
        {
            TimeManager.Instance.RequestResume();
            if (playerController != null)
                playerController.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
{
        // Solo recoge objetos que tengan el componente PickupItem
        var pickup = other.GetComponent<PickupItem>();
        if (pickup != null)
        {
            AddItem(pickup);
        }
    }


    public void AddItem(PickupItem pickup)
    {
        // 1. Intenta stackear en un slot existente con el mismo tipo de item
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (slot != null && !slot.empty && slot.itemSlot != null && slot.itemSlot.itemData == pickup.itemData)
            {
                slot.itemSlot.quantity += pickup.amount;
                slot.UpdateSlot();
                pickup.gameObject.SetActive(false);
                return;
            }
        }

        // 2. Si no hay slot stackeable, busca un slot vacío
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (slot != null && slot.empty)
            {
                InventoryItem newItem = new InventoryItem(pickup.itemData, pickup.amount);

                // Agrega a la lista correcta según el tipo
                switch (pickup.itemData.itemType)
                {
                    case ItemType.Key:
                        keys.Add(newItem);
                        break;
                    case ItemType.Collectible:
                        collectibles.Add(newItem);
                        break;
                    case ItemType.Usable:
                        usable.Add(newItem);
                        break;
                    default:
                        others.Add(newItem);
                        break;
                }

                slot.AddItem(newItem);
                pickup.gameObject.SetActive(false);
                return;
            }
        }

        // 3. Si no hay slot vacío ni stackeable, puedes mostrar mensaje de inventario lleno
        Debug.Log("Inventario lleno, no se pudo agregar el item.");
    }

    private void AddToLogicalList(PickupItem pickup)
    {
        if (pickup != null)
        {
            InventoryItem newItem = new InventoryItem(pickup.itemData, pickup.amount);

            switch (pickup.itemData.itemType)
            {
                case ItemType.Key:
                    // Si ya tienes la llave, no la agregues de nuevo
                    if (!keys.Exists(i => i.itemName == newItem.itemName))
                    {
                        keys.Add(newItem);
                        Debug.Log("Llave agregada a la lista keys: " + newItem.itemName);
                    }
                    break;

                case ItemType.Collectible:
                    collectibles.Add(newItem);
                    break;

                default:
                    others.Add(newItem);
                    break;
            }
        }
    }

     public bool RemoveItem(InventoryItemData itemData, int amount = 1)
    {
        if (itemData == null) return false;

        List<InventoryItem> targetList = others;
        if (itemData.itemType == ItemType.Key)
            targetList = keys;
        else if (itemData.itemType == ItemType.Collectible)
            targetList = collectibles;
        else if (itemData.itemType == ItemType.Usable)
            targetList = usable;

        var item = targetList.Find(i => i.itemName == itemData.itemName);
        if (item != null)
        {
            item.quantity -= amount;
            if (item.quantity <= 0)
                targetList.Remove(item);

            UpdateSlot(); // <- Esto es lo importante

            return true;
        }
        return false;
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
        // Limpia todos los slots primero
        for (int i = 0; i < allSlots; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            slot.itemSlot = null;
            slot.empty = true;
            slot.UpdateSlot();
        }

        // Junta todos los items en el orden que quieras mostrar
        int index = 0;
        List<InventoryItem> allItems = new List<InventoryItem>();
        allItems.AddRange(keys);
        allItems.AddRange(collectibles);
        allItems.AddRange(usable);
        allItems.AddRange(others);

        // Asigna los items a los slots desde el principio
        for (; index < allItems.Count && index < allSlots; index++)
        {
            Slot slot = slots[index].GetComponent<Slot>();
            slot.itemSlot = allItems[index];
            slot.empty = false;
            slot.UpdateSlot();
        }
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
            items = this.others,
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
            this.others = data.items ?? new List<InventoryItem>();
            this.collectibles = data.collectibles ?? new List<InventoryItem>();
            this.keys = data.keys ?? new List<InventoryItem>();

            // Reasignar referencias a ScriptableObjects
            InventoryItemData[] allItems = Resources.LoadAll<InventoryItemData>("");
            foreach (var item in others)
                item.itemData = System.Array.Find(allItems, d => d.name == item.itemName);
            foreach (var item in collectibles)
                item.itemData = System.Array.Find(allItems, d => d.name == item.itemName);
            foreach (var item in keys)
                item.itemData = System.Array.Find(allItems, d => d.name == item.itemName);
        }
    }

}