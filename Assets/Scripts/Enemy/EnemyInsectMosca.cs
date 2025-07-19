using UnityEngine;

public class EnemyInsectMosca : EnemyBase
{
    private Vector3 patrolDestination;

    protected override void Start()
    {
        base.Start();
        // Si quieres asegurar que esté sobre el NavMesh, deja el warp.
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(transform.position, out hit, 50f, UnityEngine.AI.NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        GoToClosestHidePoint(); // Ir directo al hide point más cercano
    }

    protected override void NormalBehavior()
    {
        GoToClosestHidePoint();
    }

    protected override void ScaredBehavior()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < chaseDistance)
            FleeFromPlayer();
        else
            GoToClosestHidePoint();
    }

    protected override void ParalyzerBehavior()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < chaseDistance && distToPlayer > paralyzeDistance)
            agent.SetDestination(player.position);
        else if (distToPlayer <= paralyzeDistance && !isParalyzing)
            StartCoroutine(ParalyzePlayer());
        // No llames a GoToClosestHidePoint aquí, espera a que termine la parálisis
        else if (!isParalyzing)
            GoToClosestHidePoint();
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolDestination) < 1f)
            SetNewPatrolDestination();
        agent.SetDestination(patrolDestination);
        Debug.Log($"{name} SetDestination a {patrolDestination}");
    }

    void SetNewPatrolDestination()
    {
        Vector2 randomPoint = Random.insideUnitCircle * 10f;
        patrolDestination = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(patrolDestination, out hit, 50f, UnityEngine.AI.NavMesh.AllAreas))//
        {
            patrolDestination = hit.position;
            Debug.Log($"{name} nuevo destino patrulla: {patrolDestination}");
        }
        else
        {
            Debug.LogWarning($"{name} no encontró destino válido en el NavMesh.");
        }
    }

    void FleeFromPlayer()
    {
        if (hidePoints.Length == 0) return;
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
        agent.speed = speed * 2f;
        agent.SetDestination(closestHide.position);
        agent.speed = speed;
    }

    System.Collections.IEnumerator ParalyzePlayer()
    {
        isParalyzing = true;
        agent.isStopped = true; // Detén al enemigo durante la parálisis
        var pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.Paralyze(paralyzeDuration);
        }
        yield return new WaitForSeconds(paralyzeDuration);
        isParalyzing = false;
        agent.isStopped = false;
        GoToClosestHidePoint(); // Ahora sí, ve al hide point
    }
}