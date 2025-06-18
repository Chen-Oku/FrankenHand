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
        
    }

    // Update is called once per frame
    public void UpdateSlot()
    {
        //this.GetComponent<Image>().sprite = icon;
        
        if(iconImage == null)
        {
            Debug.Log("IconImage no  esta asignado en el inspector en" + gameObject.name);
            // Intenta obtener el componente Image del GameObject actual");
            iconImage = GetComponent<Image>();
        }

        if (itemSlot != null && itemSlot.itemIcon != null)
        {
            iconImage.sprite = itemSlot.itemIcon; // Asigna el icono del item al Image del slot
            iconImage.enabled = true;
            quantityText.text = itemSlot.quantity > 0 ? itemSlot.quantity.ToString() : ""; // Muestra la cantidad si es mayor a 1
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            quantityText.text = ""; // Limpia el texto de cantidad si no hay item
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.itemData != null && tooltip != null) // Verifica que itemSlot y tooltip no sean nulos
        {
            //tooltip.ShowTooltip(itemSlot.itemData.itemName, itemSlot.itemData.description, Vector2.zero); // Mostrar el tooltip con la información del item
            tooltip.ShowTooltip(itemSlot.itemData.itemName, itemSlot.itemData.description, itemSlot.itemIcon);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot == null) return;
        originalPosition = iconImage.transform.parent;
        iconImage.transform.SetParent(canvas.transform); //Mueve el icono al canvas para que este sobre todo
        iconImage.raycastTarget = false; //Desactiva el raycast para que no bloquee otros eventos
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot == null) return;
        iconImage.transform.position = eventData.position; // Mueve el icono a la posición del ratón
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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

                // Ajusta la altura al nivel del suelo usando un raycast hacia abajo
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

                Inventory inventory = Object.FindFirstObjectByType<Inventory>();
                if (inventory != null)
                {
                    inventory.items.Remove(itemSlot);
                    inventory.UpdateSlot();
                }
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
        Slot otherSlot = eventData.pointerDrag?.GetComponent<Slot>();
        if (otherSlot != null && otherSlot != this)
        {
            // Intercambia los items entre los slots
            var temp = itemSlot;
            itemSlot = otherSlot.itemSlot;
            otherSlot.itemSlot = temp;

            // Actualiza los slots
            UpdateSlot();
            otherSlot.UpdateSlot();
        }
    }


    // Con estos metodos hago que el tooltip se muestre al pasar el mouse por encima del slot
    /* public void OnPointerClick(PointerEventData eventData)
    {
        if (itemSlot != null && itemSlot.itemData != null && tooltip != null)
        {
            // Mostrar el tooltip con la información del item
            tooltip.ShowTooltip(itemSlot.itemData.itemName, itemSlot.itemData.description, Vector2.zero);
        }
        {
            tooltip.ShowTooltip(itemSlot.itemData.itemName, itemSlot.itemData.description, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    } */
}

