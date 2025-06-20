using UnityEngine;

public class Falling : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                player.RespawnAtCheckpoint();
            }
        }
        Debug.Log("Falling trigger entered by: " + other.name);
    }
}
