using UnityEngine;

public class Falling : MonoBehaviour
{
    public bool permitirRespawn = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VidaPlayer vida = other.GetComponentInParent<VidaPlayer>();
            if (vida != null)
            {
                vida.RecibirDanio(1); // Cantidad de fragmentos de vida a perder 
            }
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null && permitirRespawn)
            {
                player.RespawnAtCheckpoint();
            }
        }
        //Debug.Log("Falling trigger entered by: " + other.name);
    }
}
