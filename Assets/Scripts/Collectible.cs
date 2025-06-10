using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int pointValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.CollectItem(this);
            Destroy(gameObject);
        }
    }
}
