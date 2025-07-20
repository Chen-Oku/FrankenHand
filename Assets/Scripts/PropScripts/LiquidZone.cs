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
        if (player != null && player.movement != null)
        {
            // Reduce movement speed, jump height, disable dash/run
            player.movement.SetSpeedMultiplier(slowMultiplier);
            player.movement.SetJumpMultiplier(jumpMultiplier);
            player.movement.DisableDash();
            player.movement.DisableRun();
        }

        // Damage over time
        if (vida != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                vida.RecibirDanio(damageAmount);
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && player.movement != null)
        {
            // Restore normal movement
            player.movement.SetSpeedMultiplier(1f);
            player.movement.SetJumpMultiplier(1f);
            player.movement.EnableDash();
            player.movement.EnableRun();
            damageTimer = 0f;
        }
    }
}