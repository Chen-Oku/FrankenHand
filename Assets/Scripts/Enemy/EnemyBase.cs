using UnityEngine;
using UnityEngine.AI;

public enum EnemyBehaviorType { Normal, Scared, Paralyzer }

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyBehaviorType behaviorType;
    public Transform[] hidePoints;
    public float speed = 3f;
    public float chaseDistance = 8f;
    public float paralyzeDistance = 2f;
    public float paralyzeDuration = 1f;

    protected Transform player;
    protected NavMeshAgent agent;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected bool isParalyzing = false;

    public EnemySpawner spawner;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        GoToClosestHidePoint(); // Ir directo al hide point más cercano al aparecer
    }

    protected virtual void Update()
    {
        // Sprite siempre mira a la cámara
        if (spriteRenderer != null && Camera.main != null)
        {
            Vector3 camPos = Camera.main.transform.position;
            Vector3 lookDir = camPos - spriteRenderer.transform.position;
            lookDir.y = 0; // Solo rota en Y
            if (lookDir != Vector3.zero)
                spriteRenderer.transform.forward = lookDir;

            // FlipX según dirección de movimiento (para sprites que miran a la izquierda por defecto)
            if (agent != null && agent.velocity.sqrMagnitude > 0.01f)
                spriteRenderer.flipX = agent.velocity.x > 0.01f;
        }

        switch (behaviorType)
        {
            case EnemyBehaviorType.Normal:
                NormalBehavior();
                break;
            case EnemyBehaviorType.Scared:
                ScaredBehavior();
                break;
            case EnemyBehaviorType.Paralyzer:
                ParalyzerBehavior();
                break;
        }

        if (EnHidePoint())
        {
            spawner.OnEnemyHidden(this);
            Destroy(gameObject);
        }
    }

    public virtual void SetRandomBehavior()
    {
        behaviorType = (EnemyBehaviorType)Random.Range(0, 3);
    }

    protected abstract void ScaredBehavior();
    protected abstract void ParalyzerBehavior();
    protected abstract void NormalBehavior();

    protected virtual bool EnHidePoint()
    {
        foreach (Transform hidePoint in hidePoints)
        {
            if (Vector3.Distance(transform.position, hidePoint.position) < 1f)
            {
                return true;
            }
        }
        return false;
    }

    protected void GoToClosestHidePoint()
    {
        if (hidePoints == null || hidePoints.Length == 0) return;
        Transform closestHidePoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform hidePoint in hidePoints)
        {
            float distance = Vector3.Distance(transform.position, hidePoint.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHidePoint = hidePoint;
            }
        }

        if (closestHidePoint != null)
        {
            agent.SetDestination(closestHidePoint.position);
        }
    }
}