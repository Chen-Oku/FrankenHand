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
<<<<<<< Updated upstream
                // Aquí puedes reproducir un sonido o animación de recogida
=======

                if (itemData.pickupSound != null)
                {
                    Debug.Log("Reproduciendo sonido de pickup: " + itemData.pickupSound.name);
                    AudioSource.PlayClipAtPoint(itemData.pickupSound, Camera.main.transform.position);
                }

                if (itemData.esNota)
                {
                    NoteUIManager.Instance.ShowNote(itemData.noteVideo);
                    // Puedes ocultar la nota tras un tiempo o al presionar una tecla
                }

>>>>>>> Stashed changes
                Destroy(gameObject);
            }
        }
    }
}