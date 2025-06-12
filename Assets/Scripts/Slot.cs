using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject item;
    public int ID;
    public string description;

    public bool empty;
    public Sprite icon;

    public Transform slotIconGameObject;

    public enum ItemType { Consumable, Equipment, Quest, Misc, Collectible }
    public ItemType type;

    private void Start()
    {
        slotIconGameObject = transform.GetChild(0);
    }
    public void UpdateSlot()
    {
        slotIconGameObject.GetComponent<Image>().sprite = icon;
    }

    public void UseItem()
    {

    }

}
