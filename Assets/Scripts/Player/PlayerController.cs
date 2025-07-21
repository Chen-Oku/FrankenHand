using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerInteraction interaction;
    public PlayerAnimationController animationController;
    public PlayerRespawn respawn;
    public PlayerCameraEffect cameraEffect;
    public VidaPlayer vidaPlayer;
    public SpriteRenderer spriteRenderer; 

    private Coroutine flashCoroutine;
    private Color originalColor;

    public float knockbackForce = 20f;
    public float knockbackDuration = 0.25f;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
        animationController = GetComponent<PlayerAnimationController>();
        respawn = GetComponent<PlayerRespawn>();
        cameraEffect = GetComponent<PlayerCameraEffect>();
        vidaPlayer = GetComponent<VidaPlayer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        if (vidaPlayer != null)
            vidaPlayer.RecibirDanio(amount);
        animationController?.TriggerTakeDamage();
    }

    public void Paralyze(float duration)
    {
        movement.Paralyze(duration);
        cameraEffect?.StartCinemachineShake(duration, 2f);
    }

    public void ForzarIdle()
    {
        if (animationController != null)
            animationController.ForzarIdle();
    }

    public Vector3 Velocity
    {
        get { return movement != null ? movement.GetVelocity() : Vector3.zero; }
        set { if (movement != null) movement.SetVelocity(value); }
    }

    public void RespawnAtCheckpoint()
    {
        if (respawn != null)
            respawn.RespawnAtCheckpoint();
    }

    public bool puedeMover
    {
        get => movement != null && movement.canMove;
        set { if (movement != null) movement.SetCanMove(value); }
    }

    public void SetSpeedMultiplier(float value)
    {
        if (movement != null)
            movement.SetSpeedMultiplier(value);
    }

    public void SetJumpMultiplier(float value)
    {
        if (movement != null)
            movement.SetJumpMultiplier(value);
    }

    public void DisableDash()
    {
        if (movement != null)
            movement.DisableDash();
    }

    public void EnableDash()
    {
        if (movement != null)
            movement.EnableDash();
    }

    public void DisableRun()
    {
        if (movement != null)
            movement.DisableRun();
    }

    public void EnableRun()
    {
        if (movement != null)
            movement.EnableRun();
    }

    public void ApplyKnockback(Vector3 direction, float? force = null, float? duration = null)
    {
        if (movement != null)
            movement.ApplyKnockback(
                direction,
                force ?? knockbackForce,
                duration ?? knockbackDuration
            );
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * 5f, ForceMode.Impulse);
        }
    }

    public void FlashRed(float duration = 0.5f, int flashes = 3)
    {
        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                spriteRenderer.color = originalColor;
            }
            flashCoroutine = StartCoroutine(FlashRedCoroutine(duration, flashes));
        }
    }

    private IEnumerator FlashRedCoroutine(float duration, int flashes)
    {
        for (int i = 0; i < flashes; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(duration / (flashes * 2));
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(duration / (flashes * 2));
        }
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}