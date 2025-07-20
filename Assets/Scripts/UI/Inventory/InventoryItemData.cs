using UnityEngine;
using UnityEngine.Video;

public enum ItemType { Collectible, Key, Usable, Other }

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public AudioClip pickupSound;
    public GameObject itemPrefab; // Prefab for the item, if applicable

    [TextArea]
    public string description;
    public int maxStack = 5; // 1 = no apilable, >1 = apilable

    public bool esNota;

    // NUEVO: Campos opcionales para nota multimedia
    public VideoClip notaVideo;
    public Sprite notaImagen;
    public float imagenMostrarTiempo = 5f; // Tiempo que se muestra la imagen

    public static int CountNotas(Inventory inventory)
    {
        int count = 0;
        foreach (var item in inventory.collectibles)
        {
            if (item.itemData != null && item.itemData.esNota)
                count++;
        }
        return count;
    }
}