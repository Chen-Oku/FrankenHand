using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null && itemData != null)
            {
                //inventory.AddItem(itemData, amount);

                // Mostrar la nota si corresponde
                if (itemData.esNota)
                {
                    // Busca el NotaCounterUI en la escena y muestra la nota
                    NotaCounterUI notaUI = FindFirstObjectByType<NotaCounterUI>();
                    if (notaUI != null)
                        notaUI.MostrarNota(itemData);
                }

                if (itemData.pickupSound != null)
                {
                    Debug.Log("Reproduciendo sonido de pickup: " + itemData.pickupSound.name);
                    AudioSource.PlayClipAtPoint(itemData.pickupSound, Camera.main.transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}