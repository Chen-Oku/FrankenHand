using UnityEngine;

public class HoleSnapTrigger : MonoBehaviour
{
    public Transform snapPosition;
    public string libroTag = "Libro";
    public ParticleSystem efectoPolvo;

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
        }
    }
}
