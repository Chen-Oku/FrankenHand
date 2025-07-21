using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones optionsPanel;
    public GameObject previousMenu;

    public void ShowOptionsMenu()
    {
        if (optionsPanel != null)
            optionsPanel.pantallaOpciones.SetActive(true);
        else
            Debug.LogError("No se asignó el ControladorOpciones en el Inspector.");
    }
}
