using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // Asigna el Inventory del jugador en el Inspector
    public TextMeshProUGUI inventoryText;  // Asigna un TextMeshProUGUI en el Inspector

    void Update()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in inventory.items)
        {
            sb.AppendLine(item.name);
        }
        inventoryText.text = sb.ToString();
    }
}