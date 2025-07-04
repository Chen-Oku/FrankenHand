using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName; // Guardar el nombre del ScriptableObject para serialización
    public int quantity;
    public bool pickedcUp;
    public Sprite itemIcon;

    [System.NonSerialized]
    public InventoryItemData itemData; // Asignar después de cargar

    public InventoryItem(InventoryItemData data, int quantity = 1)
    {
        this.itemName = data.itemName;
        this.itemIcon = data.icon;
        this.quantity = quantity;
        this.itemData = data;
    }
}