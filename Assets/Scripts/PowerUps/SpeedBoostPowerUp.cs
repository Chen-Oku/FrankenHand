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

        float originalSpeed = player.speed;
        player.speed = originalSpeed * boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        player.speed = originalSpeed;
        // Aquí termina el efecto, el objeto sigue oculto
    }
}
