using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;
    public GameObject inventoryUI; // Asigna el objeto de la UI del inventario
    public GameObject[] slotsNotas;
    public GameObject[] slotsGeneral;
    public GameObject slotHolderNotas; // Asigna en el inspector el objeto padre de los slots de notas
    public GameObject slotHolderGeneral; // Asigna en el inspector

    private PlayerController playerController; // Referencia al script de control del jugador

    public List<InventoryItem> others = new List<InventoryItem>();
    public List<InventoryItem> usable = new List<InventoryItem>();
    public List<InventoryItem> collectibles = new List<InventoryItem>();
    public List<InventoryItem> keys = new List<InventoryItem>();


    public int maxSlots = 20;

    public PauseMenu pauseMenu; // Asigna el PauseMenu desde el Inspector
    public CanvasGroup inventoryCanvasGroup; // Asigna el CanvasGroup desde el Inspector

    public const int MAX_NOTAS = 5;

    void Start()
    {
        inventoryEnabled = false;

        if (inventoryCanvasGroup == null && inventoryUI != null)
            inventoryCanvasGroup = inventoryUI.GetComponent<CanvasGroup>();

        // Oculta el inventario al iniciar
        if (inventoryCanvasGroup != null)
            SetInventoryUIVisible(false);

        playerController = Object.FindFirstObjectByType<PlayerController>();
        if (playerController != null)
            playerController.enabled = true; // Asegúrate de que el PlayerController esté habilitado al inicio

        // Inicializa slotsNotas automáticamente
        if (slotHolderNotas != null)
        {
            int notasCount = slotHolderNotas.transform.childCount;
            slotsNotas = new GameObject[notasCount];
            for (int i = 0; i < notasCount; i++)
            {
                slotsNotas[i] = slotHolderNotas.transform.GetChild(i).gameObject;
                if (slotsNotas[i].GetComponent<Slot>().itemSlot == null)
                    slotsNotas[i].GetComponent<Slot>().empty = true;
            }
        }

        // Inicializa slotsGeneral automáticamente
        if (slotHolderGeneral != null)
        {
            int generalCount = slotHolderGeneral.transform.childCount;
            slotsGeneral = new GameObject[generalCount];
            for (int i = 0; i < generalCount; i++)
            {
                slotsGeneral[i] = slotHolderGeneral.transform.GetChild(i).gameObject;
                if (slotsGeneral[i].GetComponent<Slot>().itemSlot == null)
                    slotsGeneral[i].GetComponent<Slot>().empty = true;
            }
        }
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
                    //TimeManager.Instance.RequestResume();
                    
                }
            }
        }

        // Comprobación del menú de opciones
        var opcionesScript = Object.FindFirstObjectByType<ControladorOpciones>();
        bool opcionesAbiertas = opcionesScript != null && opcionesScript.opcionesCanvasGroup != null && opcionesScript.opcionesCanvasGroup.alpha > 0.5f;

        // Controla el PlayerController según si hay algún menú abierto
        if (playerController != null)
            playerController.enabled = !(inventoryEnabled || (pauseMenu != null && pauseMenu.IsPauseMenuActive()) || opcionesAbiertas);

        // Pausa el juego si cualquier menú está abierto
        if (AnyMenuOpen())
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public bool AnyMenuOpen()
    {
        bool inventarioAbierto = inventoryCanvasGroup != null && inventoryCanvasGroup.alpha > 0.5f;
        bool pausaAbierta = pauseMenu != null && pauseMenu.IsPauseMenuActive();

        var opcionesScript = Object.FindFirstObjectByType<ControladorOpciones>();
        bool opcionesAbiertas = opcionesScript != null && opcionesScript.opcionesCanvasGroup != null && opcionesScript.opcionesCanvasGroup.alpha > 0.5f;

        return inventarioAbierto || pausaAbierta || opcionesAbiertas;
    }


    public void CloseInventory()
    {
        inventoryEnabled = false;
        SetInventoryUIVisible(false);

        // Reanuda solo si no hay otro menú abierto
        if (!AnyMenuOpen())
            Time.timeScale = 1f;

        if (playerController != null)
            playerController.enabled = true;
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
        // --- NOTAS: No stackear y máximo 5 ---
        if (pickup.itemData.itemType == ItemType.Collectible && pickup.itemData.esNota)
        {
            int notaCount = collectibles.FindAll(i => i.itemData.esNota).Count;
            if (notaCount >= MAX_NOTAS)
            {
                Debug.Log("¡Ya tienes el máximo de " + MAX_NOTAS + " notas!");
                return;
            }

            // Busca un slot vacío SOLO en slotsNotas
            for (int i = 0; i < slotsNotas.Length; i++)
            {
                Slot slot = slotsNotas[i].GetComponent<Slot>();
                if (slot != null && slot.empty)
                {
                    InventoryItem newItem = new InventoryItem(pickup.itemData, 1);
                    collectibles.Add(newItem);
                    slot.AddItem(newItem);
                    pickup.gameObject.SetActive(false);
                    UpdateAllSlots();
                    return;
                }
            }

            Debug.Log("Inventario de notas lleno, no se pudo agregar la nota.");
            return;
        }

        // --- RESTO DE ITEMS: lógica original ---
        // 1. Intenta stackear en un slot existente con el mismo tipo de item
        for (int i = 0; i < slotsGeneral.Length; i++)
        {
            Slot slot = slotsGeneral[i].GetComponent<Slot>();
            if (slot != null && !slot.empty && slot.itemSlot != null && slot.itemSlot.itemData == pickup.itemData)
            {
                slot.itemSlot.quantity += pickup.amount;
                slot.UpdateSlot();
                pickup.gameObject.SetActive(false);
                UpdateAllSlots();
                return;
            }
        }

        // 2. Si no hay slot stackeable, busca un slot vacío en slotsGeneral
        for (int i = 0; i < slotsGeneral.Length; i++)
        {
            Slot slot = slotsGeneral[i].GetComponent<Slot>();
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
                UpdateAllSlots();
                return;
            }
        }

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
                    if (pickup.itemData.itemName == "Nota")
                    {
                        int notaCount = collectibles.FindAll(i => i.itemName == "Nota").Count;
                        if (notaCount >= MAX_NOTAS)
                        {
                            Debug.Log("¡Ya tienes el máximo de " + MAX_NOTAS + " notas!");
                            return;
                        }
                    }
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

            UpdateAllSlots(); // <- Esto es lo importante

            return true;
        }
        return false;
    } 
  
    public void DeselectAllSlots()
    {
        // Desselecciona todos los slots de notas
        for (int i = 0; i < slotsNotas.Length; i++)
        {
            Slot slot = slotsNotas[i].GetComponent<Slot>();
            if (slot != null)
            {
                slot.selectedShader.SetActive(false);
                slot.thisItemSelected = false;
            }
        }
        // Desselecciona todos los slots generales
        for (int i = 0; i < slotsGeneral.Length; i++)
        {
            Slot slot = slotsGeneral[i].GetComponent<Slot>();
            if (slot != null)
            {
                slot.selectedShader.SetActive(false);
                slot.thisItemSelected = false;
            }
        }
    }
    
    public void UpdateAllSlots()
{
    // Limpia todos los slots de notas
    for (int i = 0; i < slotsNotas.Length; i++)
    {
        Slot slot = slotsNotas[i].GetComponent<Slot>();
        slot.itemSlot = null;
        slot.empty = true;
        slot.UpdateSlot();
    }
    // Limpia todos los slots generales
    for (int i = 0; i < slotsGeneral.Length; i++)
    {
        Slot slot = slotsGeneral[i].GetComponent<Slot>();
        slot.itemSlot = null;
        slot.empty = true;
        slot.UpdateSlot();
    }

    // Asigna notas/collectibles a los slotsNotas
    int indexNota = 0;
    foreach (var item in collectibles)
    {
        if (item.itemData.itemType == ItemType.Collectible && item.itemData.esNota && indexNota < slotsNotas.Length)
        {
            Slot slot = slotsNotas[indexNota].GetComponent<Slot>();
            slot.itemSlot = item;
            slot.empty = false;
            slot.UpdateSlot();
            indexNota++;
        }
    }

    // Asigna el resto de objetos a los slotsGeneral
    int indexGeneral = 0;
    List<InventoryItem> allGeneral = new List<InventoryItem>();
    allGeneral.AddRange(keys);
    allGeneral.AddRange(usable);
    allGeneral.AddRange(others);

    for (; indexGeneral < allGeneral.Count && indexGeneral < slotsGeneral.Length; indexGeneral++)
    {
        Slot slot = slotsGeneral[indexGeneral].GetComponent<Slot>();
        slot.itemSlot = allGeneral[indexGeneral];
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