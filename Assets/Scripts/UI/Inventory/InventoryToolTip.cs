using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryTooltip : MonoBehaviour
{
    public TMP_Text nameText; // TextMeshPro para el nombre del item
    public TMP_Text descriptionText; // TextMeshPro para la descripción del item
    public GameObject panel; // Panel que contiene el tooltip

    public void ShowTooltip(string name, string description, Sprite icon)
    {
       /*  Vector2 offset = new Vector2(10, -10); // Offset para evitar que el tooltip se superponga al cursor

        nameText.text = name; // Set the name text
        descriptionText.text = description; // Set the description text
        panel.SetActive(true); // Activate the tooltip panel
        panel.transform.position = position + offset; // Set the position of the tooltip panel */

        nameText.text = name;
        descriptionText.text = description;
        panel.SetActive(true); // O mantenlo siempre activo

/*         if (itemSlot != null)
        {
            //click derecho para usar el item
        } */
            UsarItem();

        /* if (eventData.button == PointerEventData.InputButton.Right)
        {
        } */
        
    }

    public void HideTooltip()
    {
        panel.SetActive(false);
    }
    
    void UsarItem()
    {
        // Aquí puedes implementar la lógica para usar el item

        Debug.Log("Usando item: " + nameText.text);
    }

    /* public void HideTooltip()
    {
        panel.SetActive(false);
    } */
}