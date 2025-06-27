using UnityEngine;

public class OutlineActivator : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        // Busca el componente Outline en este objeto o en sus hijos
        outline = GetComponent<Outline>();
        if (outline == null)
            outline = GetComponentInChildren<Outline>();

        if (outline != null)
            outline.enabled = false; // Desactiva el outline al inicio
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && outline != null)
        {
            outline.enabled = false;
        }
    }
}