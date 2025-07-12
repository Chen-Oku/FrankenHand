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
            if (item.itemName == keyItemName)
            {
                hasKey = true;
                break;
            }
        }

        keyIconImage.sprite = hasKey ? keyIconSprite : keySilhouetteSprite;
        keyIconImage.enabled = true; // Siempre visible
    }
    
}