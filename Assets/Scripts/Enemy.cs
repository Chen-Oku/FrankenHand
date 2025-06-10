using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float chaseDistance = 10f;
    public int pointValue = 20;
    private PlayerController player;

    void Start()
    {
        // Buscar al jugador en la escena
        player = Object.FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        // Verificar si el jugador está dentro de la distancia de persecución
        if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
        {
            // Perseguir al jugador (puedes usar NavMeshAgent para 3D)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 3f);
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
