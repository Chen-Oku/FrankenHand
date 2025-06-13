using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public enum ItemType { Consumable, Equipment, Quest, Misc, Collectible }
    public ItemType type;
    public string description;
    public Sprite icon;


    [HideInInspector]
    public bool pickedUp;
    public bool equipped;


    private void Update()
    {
        if (equipped)
        {
            // Logic for equipped items can be added here
            Debug.Log("Item is equipped: " + type);
        }
    }
    
    public void UseItem()
    {
        if (type == ItemType.Consumable)
        {
    
        }
        // Implement item usage logic here
        Debug.Log("Using item: " + type);
    }
}
