using UnityEngine;

public class JumpBoostPowerUp : MonoBehaviour
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
        if (player != null && player.movement != null)
        {
            StartCoroutine(ApplyJumpBoost(player.movement));
        }
    }

    private System.Collections.IEnumerator ApplyJumpBoost(PlayerMovement movement)
    {
        isAvailable = false;
        if (visualEffect != null) visualEffect.SetActive(false);
        if (icon != null) icon.SetActive(false);
        gameObject.SetActive(false); // Oculta el power-up

        float originalJumpHeight = movement.jumpHeight;
        movement.jumpHeight = originalJumpHeight * boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        movement.jumpHeight = originalJumpHeight;
        // Aquí termina el efecto, el objeto sigue oculto
    }
}
