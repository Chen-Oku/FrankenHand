using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public bool inventoryEnabled;

    public GameObject inventory;
    private int allSlots;
    private int enableSlots;
    private GameObject[] slot;
    public GameObject slotHolder;

    public List<Item> items = new List<Item>();
    public List<Item> collectibles = new List<Item>();


    void Start()
    {
        allSlots = slotHolder.transform.childCount;

        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if(slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
            inventory.SetActive(inventoryEnabled);

            // Pausar o reanudar el juego
            Time.timeScale = inventoryEnabled ? 0f : 1f;

        }

         // Opcional: asegura que el inventario esté activo/inactivo según el estado
        inventory.SetActive(inventoryEnabled);
    
        if (inventoryEnabled == true)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            GameObject itemPickedUp = other.gameObject;
            Item item = itemPickedUp.GetComponent<Item>();
            AddItem(itemPickedUp, item.ID, item.type.ToString(), item.description, item.icon);

        }
    }

    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().type = (Slot.ItemType)System.Enum.Parse(typeof(Slot.ItemType), itemType);
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().icon = itemIcon;

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();

                slot[i].GetComponent<Slot>().empty = false;

                return;

            }
        }

    }

    public void AddCollectible(Item item)
    {
        if (item.type == Item.ItemType.Collectible)
        {
            collectibles.Add(item);
            collectibles = collectibles.OrderBy(i => i.ID).ToList(); // Mantén el orden
        }
        else
        {
            items.Add(item);
        }
    }
}
