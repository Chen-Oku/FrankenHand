using UnityEngine;

public class Coin : MonoBehaviour
{
    public int pointValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.CollectCoin(this);
            Destroy(gameObject);
        }
    }
}
