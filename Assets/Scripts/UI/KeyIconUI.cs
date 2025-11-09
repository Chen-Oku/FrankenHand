using UnityEngine;
using UnityEngine.UI;

public class KeyIconUI : MonoBehaviour
{
    public Inventory inventory;
    public string keyItemName = "Key_LlavePuerta";

    // Image del padre (silueta)
    public Image silhouetteImage;
    // Image hijo (icono de la llave) -> se activa cuando se obtiene la llave
    public Image keyIconImage;

    public Sprite keySilhouetteSprite;
    public Sprite keyIconSprite;

    void Awake()
    {
        // si no están asignadas en el inspector, intentamos obtenerlas
        if (silhouetteImage == null) silhouetteImage = GetComponent<Image>();
    }

    void Update()
    {
        if (inventory == null || silhouetteImage == null || keyIconImage == null) return;

        bool hasKey = false;
        foreach (var item in inventory.keys)
        {
            if (item.itemName == keyItemName)
            {
                hasKey = true;
                break;
            }
        }

        // la silueta siempre muestra la sprite de silueta (puedes cambiar esto si quieres reemplazarla)
        silhouetteImage.sprite = keySilhouetteSprite;

        // activamos el image hijo sólo cuando se obtiene la llave
        keyIconImage.enabled = hasKey;

        // opcional: si quieres que el hijo muestre otra sprite, asegúrate de asignarla
        if (hasKey) keyIconImage.sprite = keyIconSprite;
    }
    
}