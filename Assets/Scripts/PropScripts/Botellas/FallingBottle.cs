using UnityEngine;

public class FallingBottle : MonoBehaviour
{
    public GameObject shadowIndicator;
    public float fallDelay = 1.5f;
    public bool fallOnPlayerApproach = false;
    public float triggerRadius = 3f;
    public float shadowBlinkSpeed = 8f; // Velocidad del parpadeo
    public float extraGravity = 20f;    // Gravedad extra para acelerar la caída
    public int damagedealt = 1; // Daño al jugador al caer

    private Rigidbody rb;
    private bool hasFallen = false;
    private float shadowBlinkTimer = 0f;
    private bool shadowBlinking = false;
    private bool damageDealt = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        if (shadowIndicator != null)
            shadowIndicator.SetActive(true); // <-- Cambia a true para que siempre esté visible

        if (!fallOnPlayerApproach)
            Invoke(nameof(ShowIndicatorAndFall), Random.Range(1f, 5f)); // Caída aleatoria
    }

    // Update is called once per frame
    void Update()
    {
        if (fallOnPlayerApproach && !hasFallen)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, triggerRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    ShowIndicatorAndFall();
                    break;
                }
            }
        }

        // Parpadeo de la sombra
        if (shadowBlinking && shadowIndicator != null)
        {
            shadowBlinkTimer += Time.deltaTime * shadowBlinkSpeed;
            float alpha = Mathf.Abs(Mathf.Sin(shadowBlinkTimer));
            var renderer = shadowIndicator.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color c = renderer.material.color;
                c.a = Mathf.Lerp(0.3f, 1f, alpha); // Ajusta los valores para el parpadeo
                renderer.material.color = c;
            }
        }

        // Gravedad extra durante la caída
        if (rb != null && !rb.isKinematic)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void ShowIndicatorAndFall()
    {
        if (shadowIndicator != null)
        {
            shadowIndicator.SetActive(true);
            shadowBlinking = true;
        }
        Invoke(nameof(TriggerFall), fallDelay);
        hasFallen = true;
    }

    void TriggerFall()
    {
        if (shadowIndicator != null)
        {
            shadowIndicator.SetActive(false);
            shadowBlinking = false;
            shadowBlinkTimer = 0f;
        }
        rb.isKinematic = false; // Activa la física para que caiga
        rb.linearVelocity = Vector3.zero; // Reinicia velocidad por si acaso
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!rb.isKinematic)
        {
            // Daño al jugador si la botella lo golpea (solo una vez)
            if (!damageDealt && collision.gameObject.CompareTag("Player"))
            {
                var vida = collision.gameObject.GetComponent<SistemaVida>();
                if (vida != null)
                {
                    vida.RecibirDanio(damagedealt);
                    damageDealt = true;
                }
            }

            // Oculta la botella original
            foreach (var renderer in GetComponentsInChildren<Renderer>())
                renderer.enabled = false;

            // Oculta la sombra al romperse la botella
            if (shadowIndicator != null)
                shadowIndicator.SetActive(false);

            // Destruye la botella original después de un pequeño delay para evitar dobles colisiones
            Destroy(gameObject, 0.1f); // Destruye la botella original después de un pequeño delay
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un gizmo de esfera para visualizar el área de activación
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
