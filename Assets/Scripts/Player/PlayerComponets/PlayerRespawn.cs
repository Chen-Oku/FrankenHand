using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Checkpoint lastCheckpoint;
    public bool permitirRespawn = true;

    private CharacterController charController;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null)
            lastCheckpoint = checkpoint;
    }

    public void RespawnAtCheckpoint(string source = "Desconocido")
    {
        if (!permitirRespawn) return;

        if (lastCheckpoint != null)
        {
            if (charController != null)
                charController.enabled = false;

            transform.position = lastCheckpoint.transform.position;

            if (charController != null)
                charController.enabled = true;
        }
        else
        {
            PlayerSpawnPoint spawn = Object.FindFirstObjectByType<PlayerSpawnPoint>();
            if (spawn != null)
            {
                if (charController != null)
                    charController.enabled = false;

                transform.position = spawn.transform.position;

                if (charController != null)
                    charController.enabled = true;
            }
        }
    }
}