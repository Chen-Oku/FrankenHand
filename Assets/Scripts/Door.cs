using UnityEngine;

public class Door : MonoBehaviour
{
    public Inventory inventory;
    public InventoryItemData requiredKeyData; // Asigna el ScriptableObject de la llave en el Inspector

    public void TryOpen()
    {
        // Intenta remover la llave del inventario
        if (inventory.RemoveItem(requiredKeyData, 1))
        {
            Debug.Log("Puerta abierta");
            // Aquí tu lógica de animación o destrucción
        }
        else
        {
            Debug.Log("Necesitas la llave para abrir esta puerta.");
        }
    }
}
