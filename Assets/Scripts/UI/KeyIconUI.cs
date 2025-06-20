using UnityEngine;
using UnityEngine.UI;

public class KeyIconUI : MonoBehaviour
{
    public Inventory inventory;
    public string keyItemName = "Key_LlavePuerta";
    public Image keyIconImage;
    public Sprite keySilhouetteSprite;
    public Sprite keyIconSprite;

    void Update()
    {
        bool hasKey = false;
        foreach (var item in inventory.keys)
        {
            Debug.Log("Llave en inventario: " + item.itemName);
            if (item.itemName == keyItemName)
            {
                Debug.Log("¡Llave encontrada!");
                hasKey = true;
                break;
            }
        }

        keyIconImage.sprite = hasKey ? keyIconSprite : keySilhouetteSprite;
        keyIconImage.enabled = true; // Siempre visible
    }
    
}