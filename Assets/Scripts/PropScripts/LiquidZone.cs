using UnityEngine;

public class LiquidZone : MonoBehaviour
{
    public float slowMultiplier = 0.5f;
    public float jumpMultiplier = 0.7f;
    public float damageInterval = 2f;
    public int damageAmount = 10;

    private float damageTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        VidaPlayer vida = other.GetComponent<VidaPlayer>();
        if (player != null)
        {
            // Reduce movement speed, jump height, disable dash/run
            player.SetSpeedMultiplier(slowMultiplier);
            player.SetJumpMultiplier(jumpMultiplier);
            player.DisableDash();
            player.DisableRun();
        }

        // Damage over time
        if (vida != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                vida.RecibirDanio(damageAmount); // Asegúrate que este método exista en VidaPlayer/SistemaVida
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Restore normal movement
            player.SetSpeedMultiplier(1f);
            player.SetJumpMultiplier(1f);
            player.EnableDash();
            player.EnableRun();
            damageTimer = 0f;
        }
    }
}