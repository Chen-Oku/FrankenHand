using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player != null && player.respawn != null)
        {
            player.respawn.lastCheckpoint = this;
            // Feedback visual/audio opcional
        }
    }
}
