using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour, IAgarrable
{
    private Rigidbody rb;
    private float originalMass;
    private bool isGrabbed = false;
    private Transform grabber;
    private Collider libroCol;
    private Collider jugadorCol;

    public float grabbedMass = 20f;
    public float holdDistance = 2f; // Ajusta para que el libro no toque al jugador
    public Vector3 posicionFinal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalMass = rb.mass;
        libroCol = GetComponent<Collider>();
    }

    public void Agarrar(Transform jugador)
    {
        isGrabbed = true;
        grabber = jugador;
        rb.mass = grabbedMass;
        rb.useGravity = false;
        rb.linearDamping = 10f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        jugadorCol = jugador.GetComponent<Collider>();
        if (libroCol != null && jugadorCol != null)
            Physics.IgnoreCollision(libroCol, jugadorCol, true);

        // --- NUEVO: Reposiciona al jugador a una distancia específica del libro considerando el tamaño ---
        Vector3 direccion = (jugador.position - transform.position).normalized;

        // Calcula la extensión máxima del collider en la dirección de movimiento
        float extent = libroCol.bounds.extents.magnitude;
        float distanciaDeseada = extent + holdDistance; // suma el tamaño del objeto + la distancia deseada extra

        jugador.position = transform.position + direccion * distanciaDeseada;
    }

    public void Soltar()
    {
        isGrabbed = false;
        if (libroCol != null && jugadorCol != null)
            Physics.IgnoreCollision(libroCol, jugadorCol, false);

        grabber = null;
        jugadorCol = null;
        rb.mass = originalMass;
        rb.useGravity = true;
        rb.linearDamping = 0f;

        // Verifica si hay superficie debajo
        float checkDistance = 0.5f;
        if (!Physics.Raycast(transform.position, Vector3.down, checkDistance))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public void Arrastrar(Vector3 destino)
    {
        // Implementa aquí la lógica de arrastrar si la necesitas
    }

    void FixedUpdate()
    {
        if (isGrabbed && grabber != null)
        {
            // Calcula el destino delante del jugador, con offset arriba para evitar colisión con la mesa
            Vector3 destino = grabber.position + grabber.forward * holdDistance + Vector3.up * 0.15f;
            Vector3 nuevaPos = Vector3.Lerp(rb.position, destino, 0.3f);
            rb.MovePosition(nuevaPos);

            // Raycasts en la base para soltar si no hay suficiente apoyo
            float checkDistance = 0.6f;
            float extent = libroCol.bounds.extents.x * 0.9f;
            Vector3[] offsets = {
                new Vector3( extent, 0,  extent),
                new Vector3(-extent, 0,  extent),
                new Vector3( extent, 0, -extent),
                new Vector3(-extent, 0, -extent),
                new Vector3( 0,      0,  extent),
                new Vector3( 0,      0, -extent),
                new Vector3( extent, 0, 0),
                new Vector3(-extent, 0, 0)
            };
            int hits = 0;
            foreach (var offset in offsets)
            {
                if (Physics.Raycast(transform.position + offset, Vector3.down, checkDistance))
                    hits++;
            }
            float ratioApoyo = hits / (float)offsets.Length;
            if (ratioApoyo < 0.3f)
                Soltar();
        }
    }
}