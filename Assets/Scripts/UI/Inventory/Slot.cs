using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //public GameObject item;
    public int ID;
    public string itemData;
    public string itemName; // Nombre del ScriptableObject para serialización
    public string type;
    public int quantity;
    public string description;
    public bool empty;

    [SerializeField]
    public Image iconImage;
    public InventoryItem itemSlot;
    public TMP_Text quantityText; // Texto para mostrar la cantidad del item
    public InventoryTooltip tooltip; // Referencia al tooltip para mostrar información del item

    private Transform originalPosition; // Posición original del icono
    private Canvas canvas; // Referencia al canvas para mover el icono

    public GameObject itemPrefab; // para asignar el prefab del item si es necesario

    public GameObject selectedShader; // Shader que se activa al seleccionar el item
    public bool thisItemSelected = false; // Indica si este item está seleccionado

    private Inventory inventory; // Referencia al inventario, si es necesario


    [Header("Drop Settings")]
    public float dropSideDistance = 3f;
    public float dropUpDistance = 2f;
    public float dropRaycastDown = 10f;
    public float dropImpulse = 2f;
    public float dropColliderDelay = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        // Asegúrate de que iconImage esté asignado
        if (iconImage == null)
        {
            iconImage = GetComponent<Image>();
            if (iconImage == null)
            {
                //iconImage = transform.Find("Icon").GetComponent<Image>(); // Por si tengo un hijo llamado "Icon" con un componente Image
                Debug.Log("IconImage no está asignado en el inspector en " + gameObject.name);
            }
        }
        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<TMP_Text>();
        }
        if (tooltip == null)
        {
            tooltip = Object.FindFirstObjectByType<InventoryTooltip>();
            if (tooltip == null)
                Debug.LogWarning("No se encontró InventoryTooltip en la escena.");
        }
    }

    void Start()
    {
        inventory = Object.FindFirstObjectByType<Inventory>();
    }


    public void UpdateSlot()
    {
        if (itemSlot != null && itemSlot.itemData != null)
        {
            iconImage.sprite = itemSlot.itemData.icon;
            iconImage.enabled = true;

            // Actualiza la cantidad
            if (quantityText != null)
            {
                if (itemSlot.quantity > 1)
                    quantityText.text = itemSlot.quantity.ToString();
                else
                    quantityText.text = ""; // O "1" si prefieres mostrar siempre la cantidad
            }
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            if (quantityText != null)
                quantityText.text = "";
        }
    }

//=========================
    /*      public void AddItem(PickupItem pickup)
        {
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
            // Si no hay slot vacío, puedes intentar stackear o mostrar mensaje de inventario lleno
        } */

    /*     public void AddItem(InventoryItemData itemData, int amount)
        {
            this.itemSlot = new InventoryItem(itemData, amount);
            this.empty = false;
            UpdateSlot();
        } */

    public void AddItem(InventoryItem item)
    {
        this.itemSlot = item;
        this.empty = false;
        UpdateSlot();
    }

//=========================


    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemSlot == null || empty)
            return; // No seleccionar si el slot está vacío

        if (inventory != null)
            inventory.DeselectAllSlots();

        thisItemSelected = true;
        if (selectedShader != null)
            selectedShader.SetActive(true);

        // Mostrar tooltip solo si hay item
        if (tooltip != null && itemSlot != null && itemSlot.itemData != null)
            tooltip.ShowTooltip(itemSlot.itemData.itemName, itemSlot.itemData.description, itemSlot.itemData.icon);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot == null || empty) return;
        originalPosition = iconImage.transform.parent;
        iconImage.transform.SetParent(canvas.transform); //Mueve el icono al canvas para que este sobre todo
        iconImage.raycastTarget = false; //Desactiva el raycast para que no bloquee otros eventos
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot == null || empty) return;
        iconImage.transform.position = eventData.position; // Mueve el icono a la posición del ratón
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemSlot == null || empty) return;
        // Si se suelta el icono fuera de un slot, lo devuelve a su posición original
        iconImage.transform.SetParent(originalPosition);
        iconImage.transform.localPosition = Vector3.zero;
        iconImage.raycastTarget = true;

        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<Slot>() == null)
        {
            if (itemSlot != null && itemSlot.itemData != null && itemSlot.itemData.itemPrefab != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 dropPosition;
                Vector3 left;

                if (player != null)
                {
                    left = -player.transform.right;
                    dropPosition = player.transform.position + left * dropSideDistance + Vector3.up * dropUpDistance;
                }
                else
                {
                    left = -Camera.main.transform.right;
                    dropPosition = Camera.main.transform.position + left * dropSideDistance + Vector3.up * dropUpDistance;
                }

                RaycastHit hit;
                if (Physics.Raycast(dropPosition, Vector3.down, out hit, dropRaycastDown))
                {
                    dropPosition.y = hit.point.y + 0.1f;
                }

                GameObject dropped = Instantiate(itemSlot.itemData.itemPrefab, dropPosition, Quaternion.identity);
                Debug.Log("Prefab instanciado: " + dropped.name + " en posición " + dropPosition);

                Rigidbody rb = dropped.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(left * dropImpulse, ForceMode.Impulse);
                }

                Collider collider = dropped.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                    StartCoroutine(EnableColliderAfterDelay(collider, dropColliderDelay));
                }

                // --- AJUSTE: Solo elimina el item de este slot ---
                if (itemSlot.quantity > 1)
                {
                    itemSlot.quantity--;
                }
                else
                {
                    if (inventory != null && itemSlot != null && itemSlot.itemData != null)
                    {
                        inventory.RemoveItem(itemSlot.itemData, 1);
                    }
                    itemSlot = null;
                    empty = true;

                    // Oculta el tooltip si este slot estaba seleccionado
                    if (thisItemSelected && tooltip != null)
                        tooltip.HideTooltip();
                    thisItemSelected = false;
                    if (selectedShader != null)
                        selectedShader.SetActive(false);
                }
                UpdateSlot();
            }

            iconImage.transform.SetParent(originalPosition);
            iconImage.transform.localPosition = Vector3.zero;
        }
    }
    private IEnumerator EnableColliderAfterDelay(Collider col, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null)
            col.enabled = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Slot originSlot = eventData.pointerDrag.GetComponent<Slot>();
        if (originSlot != null && originSlot != this)
        {
            // Si este slot está vacío, mueve el item y limpia el slot de origen
            if (this.empty)
            {
                this.itemSlot = originSlot.itemSlot;
                this.empty = false;

                // Limpia el slot de origen
                originSlot.itemSlot = null;
                originSlot.empty = true;
                originSlot.thisItemSelected = false;
                if (originSlot.selectedShader != null)
                    originSlot.selectedShader.SetActive(false);
                originSlot.UpdateSlot();

                // Selecciona este slot
                this.thisItemSelected = true;
                if (selectedShader != null)
                    selectedShader.SetActive(true);

                this.UpdateSlot();
            }
            else // Si ambos slots tienen item, intercambia
            {
                InventoryItem tempItem = this.itemSlot;
                this.itemSlot = originSlot.itemSlot;
                originSlot.itemSlot = tempItem;

                // Actualiza los estados de vacío
                this.empty = (this.itemSlot == null);
                originSlot.empty = (originSlot.itemSlot == null);

                // Deselecciona ambos slots y oculta el shader
                this.thisItemSelected = false;
                if (selectedShader != null)
                    selectedShader.SetActive(false);

                originSlot.thisItemSelected = false;
                if (originSlot.selectedShader != null)
                    originSlot.selectedShader.SetActive(false);

                // Selecciona el slot de destino solo si NO está vacío
                if (!this.empty)
                {
                    this.thisItemSelected = true;
                    if (selectedShader != null)
                        selectedShader.SetActive(true);
                }

                // Actualiza la UI de ambos slots
                this.UpdateSlot();
                originSlot.UpdateSlot();
            }
        }
    }
}


