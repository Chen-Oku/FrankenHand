using UnityEngine;

public class BattleZone : MonoBehaviour
{
    public GameObject[] barriers; // Barreras a activar/desactivar
    public GameObject enemy;      // Enemigo a derrotar
    private bool battleStarted = false;
    private bool battleEnded = false;

    void Start()
    {
        // Asegúrate de que las barreras y el enemigo estén desactivados al inicio
        foreach (var barrier in barriers)
            barrier.SetActive(false);
        if (enemy != null)
            enemy.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !battleStarted)
        {
            battleStarted = true;
            Debug.Log("¡Jugador entró en la zona de batalla! Activando barreras...");
            foreach (var barrier in barriers)
            {
                if (barrier != null)
                {
                    barrier.SetActive(true); // Activa barreras
                    Debug.Log("Barrera activada: " + barrier.name);
                }
                else
                {
                    Debug.LogWarning("Una barrera no está asignada en el array.");
                }
            }
            if (enemy != null)
                enemy.SetActive(true);   // Activa enemigo
        }
    }

    void Update()
    {
        if (battleStarted && !battleEnded && (enemy == null || !enemy.activeInHierarchy))
        {
            foreach (var barrier in barriers)
                barrier.SetActive(false); // Desactiva barreras
            battleEnded = true;
            Destroy(gameObject); // Opcional: elimina el trigger
        }
    }
}