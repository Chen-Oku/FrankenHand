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

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
        animationController = GetComponent<PlayerAnimationController>();
        respawn = GetComponent<PlayerRespawn>();
        cameraEffect = GetComponent<PlayerCameraEffect>();
        vidaPlayer = GetComponent<VidaPlayer>();
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
        cameraEffect?.StartCinemachineShake(duration, 2f); // Usa el método nuevo
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * 5f, ForceMode.Impulse); // Ajusta la fuerza según lo necesites
        }
    }
}