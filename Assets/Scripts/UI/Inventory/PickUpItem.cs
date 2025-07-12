using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Objeto recogido por: " + other.name);
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null && itemData != null)
            {
                //inventory.AddItem(itemData, amount);
                // Aquí puedes reproducir un sonido o animación de recogida
                Destroy(gameObject);
            }
        }
    }
}