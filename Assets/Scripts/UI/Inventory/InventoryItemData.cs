using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public enum ItemType { Collectible, Key, Usable, Other }

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;

    public GameObject itemPrefab; // Prefab for the item, if applicable

    [TextArea]
    public string description;
    public int maxStack = 5; // 1 = no apilable, >1 = apilable
}