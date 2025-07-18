// NotaCounterUI.cs
using UnityEngine;
using TMPro;

public class NotaCounterUI : MonoBehaviour
{
    public Inventory inventory; // Asigna el Inventory en el inspector
    public TMP_Text notasText;  // Asigna el TextMeshPro en el inspector
    public string notaItemName = "Nota"; // El nombre del ScriptableObject de la nota

    void Update()
    {
        int count = 0;
        foreach (var item in inventory.collectibles)
        {
            if (item.itemName == notaItemName)
                count += item.quantity;
        }
        notasText.text = count.ToString();
    }
}