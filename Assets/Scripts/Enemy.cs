using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Normal, Scared, Paralyzer }

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    public Sprite[] visuals; // 3 visuales diferentes
    public RuntimeAnimatorController[] animators; // Asigna uno por tipo/visual
    public Transform[] spawnPoints;
    public Transform[] hidePoints;
    public float patrolRadius = 10f;
    public float speed = 3f;
    public float chaseDistance = 8f;
    public float fleeDistance = 3f;
    public float paralyzeDistance = 2f;
    public float paralyzeDuration = 1f;

    private Transform player;
    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 patrolDestination;
    private bool isParalyzing = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer.sprite = visuals[Random.Range(0, visuals.Length)];
        animator.runtimeAnimatorController = animators[Random.Range(0, animators.Length)];
        SpawnAtRandomPoint();
        SetNewPatrolDestination();
    }

    void Update()
    {
        // Sprite siempre mira a la cámara
        if (spriteRenderer != null && Camera.main != null)
            spriteRenderer.transform.forward = Camera.main.transform.forward;

        // --- NUEVO: Flip del sprite según dirección ---
        if (agent != null && agent.velocity.sqrMagnitude > 0.01f)
        {
            // Si la velocidad en X es negativa, mira a la izquierda; si es positiva, mira a la derecha
            spriteRenderer.flipX = agent.velocity.x < -0.01f;
        }

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        switch (type)
        {
            case EnemyType.Normal:
                Patrol();
                break;
            case EnemyType.Scared:
                if (distToPlayer < chaseDistance && distToPlayer > fleeDistance)
                    agent.SetDestination(player.position);
                else if (distToPlayer <= fleeDistance)
                    FleeFromPlayer();
                else
                    Patrol();
                break;
            case EnemyType.Paralyzer:
                if (distToPlayer < chaseDistance && distToPlayer > paralyzeDistance)
                    agent.SetDestination(player.position);
                else if (distToPlayer <= paralyzeDistance && !isParalyzing)
                    StartCoroutine(ParalyzePlayer());
                else
                    Patrol();
                break;
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolDestination) < 1f)
            SetNewPatrolDestination();
        agent.SetDestination(patrolDestination);
    }

    void FleeFromPlayer()
    {
        Debug.Log("Intentando huir al hide point más cercano");
        if (hidePoints.Length == 0) return;

        // Buscar el hide point más cercano
        Transform closestHide = hidePoints[0];
        float minDist = Vector3.Distance(transform.position, hidePoints[0].position);
        for (int i = 1; i < hidePoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, hidePoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                closestHide = hidePoints[i];
            }
        }

        // Aumentar velocidad temporalmente
        agent.speed = speed * 2f;
        agent.SetDestination(closestHide.position);

        agent.speed = speed;
    }

    System.Collections.IEnumerator ParalyzePlayer()
    {
        Debug.Log("Intentando paralizar al jugador");
        isParalyzing = true;
        var pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Paralyze(paralyzeDuration);
        }
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzing = false;
    }

    void SpawnAtRandomPoint()
    {
        if (spawnPoints.Length > 0)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            transform.position = spawn.position;
        }
    }

    public void Hide()
    {
        if (hidePoints.Length > 0)
        {
            Transform hide = hidePoints[Random.Range(0, hidePoints.Length)];
            agent.Warp(hide.position);
        }
    }

    void SetNewPatrolDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        patrolDestination = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(patrolDestination, out hit, patrolRadius, NavMesh.AllAreas))
            patrolDestination = hit.position;
    }
}
