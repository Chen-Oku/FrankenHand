using UnityEngine;

public class HoleSnapTrigger : MonoBehaviour
{
    public Transform snapPosition;
    public string libroTag = "Libro";
    public ParticleSystem efectoPolvo;
    public GameObject uiPanelMensaje; // Asigna el panel en el inspector

    private bool libroColocado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (libroColocado) return;
        if (other.CompareTag(libroTag))
        {
            // Snap y acomodo
            other.transform.position = snapPosition.position;
            other.transform.rotation = snapPosition.rotation;

            // Desactiva físicas
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            // Desactiva collider
            Collider col = other.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            libroColocado = true;

            // Activa partículas
            if (efectoPolvo != null)
                efectoPolvo.Play();

            // Desactiva el mensaje de interacción si existe
            DraggableObject1 draggable = other.GetComponent<DraggableObject1>();
            if (draggable != null)
            {
                // Desactiva el trigger del objeto DraggableObject1
                Collider triggerCol = draggable.GetComponent<Collider>();
                if (triggerCol != null)
                    triggerCol.enabled = false;

                // Desactiva el mensaje de interacción si existe
                if (draggable.interactMessage != null)
                    draggable.interactMessage.SetActive(false);

                // Desactiva el script DraggableObject1
                draggable.enabled = false;
            }

            // Desactiva el panel de mensaje en la UI si existe
            if (uiPanelMensaje != null)
                uiPanelMensaje.SetActive(false);
        }
    }
}
