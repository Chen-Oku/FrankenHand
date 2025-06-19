using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones optionsPanel;

    public GameObject previousMenu;

    void Start()
    {
        optionsPanel = GameObject.FindGameObjectWithTag("Opciones").GetComponent<ControladorOpciones>();
    }

    void Update()
    {
        
    }

    public void ShowOptionsMenu()
    {
        optionsPanel.pantallaOpciones.SetActive(true);
    }
}
