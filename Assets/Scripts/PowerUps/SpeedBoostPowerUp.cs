using UnityEngine;

public class SpeedBoostPowerUp : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 2f;
    public GameObject visualEffect;
    public GameObject icon;

    private bool isAvailable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isAvailable) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            StartCoroutine(ApplySpeedBoost(player));
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost(PlayerController player)
    {
        isAvailable = false;
        if (visualEffect != null) visualEffect.SetActive(false);
        if (icon != null) icon.SetActive(false);
        gameObject.SetActive(false); // Oculta el power-up

        // Guarda las velocidades originales
        float originalSpeed = player.movement.speed;
        float originalRunSpeed = player.movement.runSpeed;

        // Aplica el boost
        player.movement.speed = originalSpeed * boostMultiplier;
        player.movement.runSpeed = originalRunSpeed * boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        // Restaura las velocidades originales
        player.movement.speed = originalSpeed;
        player.movement.runSpeed = originalRunSpeed;
        // Aquí termina el efecto, el objeto sigue oculto
    }
}
