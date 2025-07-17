using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones optionsPanel;

    public GameObject previousMenu;

    void Start()
    {
        var opcionesObj = GameObject.FindGameObjectWithTag("Opciones");
        if (opcionesObj != null)
            optionsPanel = opcionesObj.GetComponent<ControladorOpciones>();
        else
            Debug.LogError("No se encontró un objeto con el tag 'Opciones'.");
    }

    void Update()
    {
        
    }

    public void ShowOptionsMenu()
    {
        optionsPanel.pantallaOpciones.SetActive(true);
    }
}
