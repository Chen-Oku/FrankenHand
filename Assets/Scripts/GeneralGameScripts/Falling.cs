using UnityEngine;

public class Falling : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VidaPlayer vida = other.GetComponentInParent<VidaPlayer>();
            if (vida != null)
            {
                vida.RecibirDanio(1); // O la cantidad que quieras
            }
            PlayerController player = other.GetComponentInParent<PlayerController>();
<<<<<<< Updated upstream
            if (player != null)
=======
            if (player != null && permitirRespawn && player.respawn != null)
>>>>>>> Stashed changes
            {
                player.respawn.RespawnAtCheckpoint();
            }
        }
        Debug.Log("Falling trigger entered by: " + other.name);
    }
}
