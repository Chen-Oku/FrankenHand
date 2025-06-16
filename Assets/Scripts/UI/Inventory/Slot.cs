using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject item;
    public int ID;
    public string itemData;
    public string itemName; // Nombre del ScriptableObject para serialización
    public string type;
    public int quantity;
    public string description;

    public bool empty;
    public Sprite icon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateSlot()
    {
        this.GetComponent<Image>().sprite = icon;
        
    }
}
