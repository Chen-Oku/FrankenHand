using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject1 : MonoBehaviour, IAgarrable
{
    public float weight = 3f;
    public float grabbedMass = 5f; // Masa temporal cuando está agarrado
    private float originalMass;
    private Rigidbody rb;
    private Transform grabber;

    public GameObject interactMessage; // Asigna el objeto de mensaje en el inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalMass = rb.mass;
    }

    public void Agarrar(Transform agarrador)
    {
        grabber = agarrador;
        rb.useGravity = false;
        rb.isKinematic = false; // Permite que la física actúe
        rb.linearDamping = 10f; // Damping para que no se deslice demasiado
        rb.mass = grabbedMass; // Masa baja para que el jugador lo pueda empujar
    }

    public void Soltar()
    {
        grabber = null;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 0f;
        rb.mass = originalMass;
        rb.linearVelocity = Vector3.zero;
    }

    public void Arrastrar(Vector3 direccion)
    {
        // Vacío: el jugador lo empuja con su cuerpo, no por código
    }

    void FixedUpdate()
    {
        // No muevas el objeto aquí. El jugador lo empuja con su cuerpo.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return; // Evita activar el mensaje si el script está desactivado
        if (other.CompareTag("Player") && interactMessage != null)
            interactMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactMessage != null)
            interactMessage.SetActive(false);
    }
}