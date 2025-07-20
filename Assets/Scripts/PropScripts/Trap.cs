using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null && player.respawn != null)
        {
            player.respawn.RespawnAtCheckpoint();
        }
    }
}
