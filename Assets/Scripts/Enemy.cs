using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float chaseDistance = 10f;
    public int pointValue = 20;
    public float patrolRadius = 15f;
    public float patrolWaitTime = 2f;
    private PlayerController player;
    private Vector3 patrolDestination;
    private float patrolTimer;
    private NavMeshAgent agent;

    void Start()
    {
        // Buscar al jugador en la escena
        player = Object.FindAnyObjectByType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        SetNewPatrolDestination();
        patrolTimer = 0f;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Guardar la posición anterior para detectar cambio de dirección en X
        float prevX = transform.position.x;

        if (distanceToPlayer < chaseDistance)
        {
            // Perseguir al jugador
            Vector3 targetPos = player.transform.position;
            if (agent != null)
            {
                agent.SetDestination(targetPos);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 3f);
            }
        }
        else
        {
            // Patrullar aleatoriamente
            if (agent != null)
            {
                if (Vector3.Distance(transform.position, patrolDestination) < 1f)
                {
                    patrolTimer += Time.deltaTime;
                    if (patrolTimer >= patrolWaitTime)
                    {
                        SetNewPatrolDestination();
                        patrolTimer = 0f;
                    }
                }
                else
                {
                    agent.SetDestination(patrolDestination);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, patrolDestination) < 1f)
                {
                    patrolTimer += Time.deltaTime;
                    if (patrolTimer >= patrolWaitTime)
                    {
                        SetNewPatrolDestination();
                        patrolTimer = 0f;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, patrolDestination, Time.deltaTime * 2f);
                }
            }
        }

        // Solo girar en Y si cambia de dirección en X
        float deltaX = transform.position.x - prevX;
        if (Mathf.Abs(deltaX) > 0.01f)
        {
            // Mirar hacia la cámara (jugador) solo en el eje Y
            Vector3 lookDirection = player.transform.position - transform.position;
            lookDirection.y = 0; // Solo rotación en Y
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            }
        }
    }

    private void SetNewPatrolDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        patrolDestination = new Vector3(transform.position.x + randomPoint.x, transform.position.y, transform.position.z + randomPoint.y);
        // Si usas NavMeshAgent, asegúrate de que el destino esté en el NavMesh
        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(patrolDestination, out hit, patrolRadius, NavMesh.AllAreas))
            {
                patrolDestination = hit.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que colisiona es el jugador
        var pc = other.GetComponent<PlayerController>();
        if (pc != null)// 
        {
            pc.RespawnAtCheckpoint(); // Lógica para reiniciar al jugador
        }
    }

    public void Defeat(PlayerController player)
    {
        var score = player.GetComponent<PlayerScore>();
        if (score != null)
        {
            score.AddPoints(pointValue);
        }
        // Lógica adicional al derrotar enemigo si es necesario
        Destroy(gameObject);
    }
}
