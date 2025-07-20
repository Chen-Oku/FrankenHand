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
        if (player != null)
        {
            StartCoroutine(ApplyJumpBoost(player));
        }
    }

    private System.Collections.IEnumerator ApplyJumpBoost(PlayerController player)
    {
        isAvailable = false;
        if (visualEffect != null) visualEffect.SetActive(false);
        if (icon != null) icon.SetActive(false);
        gameObject.SetActive(false); // Oculta el power-up

        float originalJumpForce = player.movement.jumpHeight;
        player.movement.jumpHeight = originalJumpForce * boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        player.movement.jumpHeight = originalJumpForce;
    }
}
