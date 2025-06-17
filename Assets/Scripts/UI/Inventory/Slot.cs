using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    //public GameObject item;
    public int ID;
    public string itemData;
    public string itemName; // Nombre del ScriptableObject para serialización
    public string type;
    public int quantity;
    public string description;
    public InventoryItem itemSlot;
    public TMP_Text quantityText; // Texto para mostrar la cantidad del item

    public bool empty;
    //public Sprite icon;

    public Image iconImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
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
}

