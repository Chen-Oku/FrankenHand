using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public List<Collectible> collectedItems = new List<Collectible>();

    public void CollectCoin(Coin coin)
    {
        GameManager.Instance.AddPoints(coin.pointValue);
        // Lógica adicional si es necesario
    }

    public void CollectItem(Collectible item)
    {
        collectedItems.Add(item);
        GameManager.Instance.AddPoints(item.pointValue); // If ambiguity persists, use: ((Collectible)item).pointValue
        // Lógica adicional si es necesario
    }

    public void UseItem(Collectible item)
    {
        if (collectedItems.Contains(item))
        {
            // Lógica para usar el item
            collectedItems.Remove(item);
            // Aquí podrías aplicar efectos del item, etc.
        }
    }
}