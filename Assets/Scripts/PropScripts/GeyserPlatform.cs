using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserPlatform : MonoBehaviour
{
    [Header("Geyser Settings")]
    public float cooldown = 2f;
    public float shakeDuration = 1f;
    public float liftHeight = 5f;
    public float liftDuration = 1f;
    public float fallDurationFactor = 1.5f;
    public float geyserForce = 30f;

    [Header("Shake Settings")]
    public float shakeAmount = 0.1f;
    public float idleShakeAmount = 0.02f;
    public float idleShakeSpeed = 2f;

    [Header("Curves")]
    public float explosionCurvePower = 0.5f;
    public float fallCurvePower = 2f;

    [Header("VFX")]
    public ParticleSystem geyserParticles;
    public Transform shakeVisual; // Asigna aquí el mesh/hijo visual del tubo

    private HashSet<Collider> playersOnPlatform = new HashSet<Collider>();
    private Coroutine geyserRoutine;
    private float lastImpulseTime = -10f;
    private bool isBusy = false;
    private bool isShaking = false;
    private Vector3 visualOriginalPos;

    private void Start()
    {
        if (geyserParticles == null)
            geyserParticles = GetComponentInChildren<ParticleSystem>();

        if (shakeVisual == null)
            shakeVisual = transform;

        visualOriginalPos = shakeVisual.localPosition;
        StartCoroutine(IdleShakeRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playersOnPlatform.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playersOnPlatform.Remove(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isBusy && Time.time - lastImpulseTime > cooldown)
        {
            if (geyserRoutine == null)
                geyserRoutine = StartCoroutine(GeyserSequence(other));
        }
    }

    private IEnumerator IdleShakeRoutine()
    {
        while (true)
        {
            if (!isShaking)
            {
                float shakePhase = Time.time * idleShakeSpeed;
                shakeVisual.localPosition = visualOriginalPos + new Vector3(
                    Mathf.Sin(shakePhase) * idleShakeAmount,
                    Mathf.Cos(shakePhase) * idleShakeAmount,
                    0f
                );
            }
            yield return null;
        }
    }

    private IEnumerator GeyserSequence(Collider playerCollider)
    {
        isBusy = true;
        isShaking = true;

        geyserParticles?.Play();

        // Shake intenso previo al impulso
        float shakeEndTime = Time.time + shakeDuration;
        while (Time.time < shakeEndTime)
        {
            shakeVisual.localPosition = visualOriginalPos + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }

        shakeVisual.localPosition = visualOriginalPos;
        isShaking = false;
        yield return new WaitForSeconds(0.2f);

        // Cancelar si el jugador se ha bajado antes del impulso
        if (!playersOnPlatform.Contains(playerCollider))
        {
            CancelGeyser();
            yield break;
        }

        // Impulso vertical al jugador
        PlayerController player = playerCollider.GetComponent<PlayerController>();

        float elapsed = 0f;
        while (elapsed < liftDuration)
        {
            float t = elapsed / liftDuration;
            float curve = Mathf.Sin(t * Mathf.PI * explosionCurvePower);
            float impulse = Mathf.Lerp(geyserForce, geyserForce * 0.3f, t);

            if (player != null)
            {
                // Aplica el impulso vertical directamente al PlayerController
                var vel = player.Velocity;
                vel.y = impulse * curve;
                player.Velocity = vel;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        lastImpulseTime = Time.time;

        // Espera en la cima
        yield return new WaitForSeconds(1f);

        geyserParticles?.Stop();
        geyserRoutine = null;
        isBusy = false;
    }

    private void CancelGeyser()
    {
        geyserParticles?.Stop();
        geyserRoutine = null;
        isBusy = false;
        shakeVisual.localPosition = visualOriginalPos;
    }
}






/* 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserPlatform : MonoBehaviour
{
    [Header("Geyser Settings")]
    public float cooldown = 2f;
    public float shakeDuration = 1f;
    public float liftHeight = 5f;
    public float liftDuration = 1f;
    public float fallDurationFactor = 1.5f;

    [Header("Shake Settings")]
    public float shakeAmount = 0.1f;
    public float idleShakeAmount = 0.02f;
    public float idleShakeSpeed = 2f;

    [Header("Curves")]
    public float explosionCurvePower = 0.5f;
    public float fallCurvePower = 2f;

    [Header("VFX")]
    public ParticleSystem geyserParticles;

    private Vector3 originalPosition;
    private HashSet<Collider> playersOnPlatform = new HashSet<Collider>();

    private Coroutine geyserRoutine;
    private float lastImpulseTime = -10f;
    private bool isBusy = false;
    private bool isShaking = false;

    private void Start()
    {
        originalPosition = transform.position;

        if (geyserParticles == null)
            geyserParticles = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(IdleShakeRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playersOnPlatform.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playersOnPlatform.Remove(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isBusy && Time.time - lastImpulseTime > cooldown)
        {
            if (geyserRoutine == null)
                geyserRoutine = StartCoroutine(GeyserSequence(other));
        }
    }

    private IEnumerator IdleShakeRoutine()
    {
        while (true)
        {
            if (!isShaking)
            {
                float shakePhase = Time.time * idleShakeSpeed;
                transform.position = originalPosition + new Vector3(
                    Mathf.Sin(shakePhase) * idleShakeAmount,
                    Mathf.Cos(shakePhase) * idleShakeAmount,
                    0f
                );
            }
            yield return null;
        }
    }

    private IEnumerator GeyserSequence(Collider playerCollider)
    {
        isBusy = true;
        isShaking = true;

        geyserParticles?.Play();

        // Shake previo a la subida
        float shakeEndTime = Time.time + shakeDuration;
        while (Time.time < shakeEndTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
        yield return new WaitForSeconds(0.2f);

        // Cancelar si el jugador se ha bajado antes de la subida
        if (!playersOnPlatform.Contains(playerCollider))
        {
            CancelGeyser();
            yield break;
        }

        // Asignar al jugador como hijo de la plataforma y poner Rigidbody en kinematic
        Transform playerTransform = playerCollider.transform;
        playerTransform.SetParent(transform);
        Rigidbody playerRb = playerCollider.GetComponent<Rigidbody>();
        if (playerRb != null)
            playerRb.isKinematic = true;
        bool playerAttached = true;

        // Subida
        Vector3 endPos = originalPosition + Vector3.up * liftHeight;
        float elapsed = 0f;

        while (elapsed < liftDuration)
        {
            float t = elapsed / liftDuration;
            float curve = Mathf.Sin(t * Mathf.PI * explosionCurvePower);
            transform.position = Vector3.Lerp(originalPosition, endPos, curve);

            // Verificar si el jugador se bajó durante la subida
            if (playerAttached && !playersOnPlatform.Contains(playerCollider))
            {
                playerTransform.SetParent(null);
                playerAttached = false;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        lastImpulseTime = Time.time;

        // Espera en la cima
        float peakWaitTime = 1f;
        float peakElapsed = 0f;
        while (peakElapsed < peakWaitTime)
        {
            // Verificar si el jugador se bajó durante la cima
            if (playerAttached && !playersOnPlatform.Contains(playerCollider))
            {
                playerTransform.SetParent(null);
                playerAttached = false;
            }

            peakElapsed += Time.deltaTime;
            yield return null;
        }

        // Liberar al jugador si aún está asignado como hijo
        if (playerAttached)
        {
            playerTransform.SetParent(null);
            if (playerRb != null)
                playerRb.isKinematic = false;
        }

        transform.position = originalPosition;

        geyserParticles?.Stop();
        geyserRoutine = null;
        isBusy = false;
    }

    private void CancelGeyser()
    {
        geyserParticles?.Stop();
        geyserRoutine = null;
        isBusy = false;
        transform.position = originalPosition;
    }
} */