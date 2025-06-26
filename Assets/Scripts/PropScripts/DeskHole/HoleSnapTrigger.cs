using UnityEngine;

public class HoleSnapTrigger : MonoBehaviour
{
    public Transform snapPosition; // Empty transform en el centro del agujero
    public string libroTag = "Libro"; // Asegúrate de ponerle este tag al libro
    public float maxSnapDistance = 0.5f; // Qué tan cerca debe estar para hacer snap
    public ParticleSystem efectoPolvo; // Asigna en el inspector si quieres partículas

    private bool libroColocado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (libroColocado) return;
        if (other.CompareTag(libroTag))
        {
            // Snap SIEMPRE que entra al trigger
            other.transform.position = snapPosition.position;
            other.transform.rotation = snapPosition.rotation;
            // Desactiva físicas
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            // Opcional: desactiva collider para que no moleste más
            Collider col = other.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            libroColocado = true;

            // Activa partículas si hay
            if (efectoPolvo != null)
                efectoPolvo.Play();
        }
    }
}
