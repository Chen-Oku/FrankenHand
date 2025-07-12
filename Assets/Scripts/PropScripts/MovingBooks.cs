using UnityEngine;

public class MovingBooks : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.forward; // Dirección de salida del libro
    public float moveDistance = 2f; // Distancia máxima de salida
    public float moveSpeed = 2f; // Velocidad de movimiento
    public float waitTime = 1f; // Tiempo de espera en cada extremo
    public float slowSpeed = 1.5f;
    public float mediumSpeed = 2.5f;
    public float fastSpeed = 4f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingOut = true;
    private float waitTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        endPos = startPos + moveDirection.normalized * moveDistance;
        // Elegir aleatoriamente una de las tres velocidades
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                moveSpeed = slowSpeed;
                break;
            case 1:
                moveSpeed = mediumSpeed;
                break;
            case 2:
                moveSpeed = fastSpeed;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        if (movingOut)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPos) < 0.01f)
            {
                movingOut = false;
                waitTimer = waitTime;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                movingOut = true;
                waitTimer = waitTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Expulsar al jugador de la plataforma
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDir = (other.transform.position - transform.position).normalized + Vector3.up;
                rb.AddForce(pushDir * 10f, ForceMode.Impulse);
            }
            // Si usas CharacterController, puedes reposicionar al jugador o llamar a un método de "caída"
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Ejemplo: reposicionar al último checkpoint
                if (playerController.lastCheckpoint != null)
                    playerController.RespawnAtCheckpoint();
            }
        }
    }
}
