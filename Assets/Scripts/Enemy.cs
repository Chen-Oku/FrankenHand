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

        // Detener toda la lógica si el juego está en pausa (por inventario o menú de pausa)
        if (Time.timeScale == 0f)
        {
                if (agent != null && !agent.isStopped)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero; // Detener movimiento inmediatamente
            }
            return;
        }
        else
        {
            if (agent != null && agent.isStopped)
            {
                agent.isStopped = false;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Guardar la posición anterior para detectar cambio de dirección en X
        // float prevX = transform.position.x; // <- Esta línea no es necesaria si no se usa

        if (distanceToPlayer < chaseDistance)
        {
            // Perseguir al jugador
            if (agent != null)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 3f);
            }
        }
        else
        {
            // Patrullar aleatoriamente
            if (agent != null)
            {
                // Si el agente está cerca del destino o está atascado, busca uno nuevo
                if (Vector3.Distance(transform.position, patrolDestination) < 1f || agent.pathPending || agent.remainingDistance < 0.5f)
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

                // Si el agente está atascado (no se mueve), fuerza un nuevo destino
                if (agent.velocity.sqrMagnitude < 0.01f && !agent.pathPending)
                {
                    SetNewPatrolDestination();
                    patrolTimer = 0f;
                }
            }
            else
            {
                // Movimiento simple si no hay NavMeshAgent
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
            else
            {
                // Si no encuentra un punto válido, intenta de nuevo
                patrolDestination = transform.position;
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
