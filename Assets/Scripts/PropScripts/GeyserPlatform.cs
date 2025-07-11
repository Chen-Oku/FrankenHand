/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserPlatform : MonoBehaviour
{
    public float geyserForce = 30f;
    public float cooldown = 2f;
    public float shakeDuration = 1f;
    public float shakeAmount = 0.1f;
    public float liftDuration = 1f;
    public float liftHeight = 5f;

    public float impulseMinFactor = 0.3f; // Mínimo de impulso al final de la subida
    public float explosionCurvePower = 0.5f; // Para la curva de subida (Mathf.Sin(t * Mathf.PI * explosionCurvePower))
    public float fallDurationFactor = 0.5f; // Duración de la caída respecto a la subida
    public float fallCurvePower = 0.7f; // Curva de aceleración de la caída

    public ParticleSystem geyserParticles;

    public float idleShakeAmount = 0.02f; // Shake suave cuando no hay jugador
    public float idleShakeSpeed = 2f;     // Velocidad del shake suave

    private float lastImpulseTime = -10f;
    private bool isShaking = false;
    private Vector3 originalPosition;
    private Coroutine geyserRoutine;
    private HashSet<Collider> playersOnPlatform = new HashSet<Collider>();
    private bool isBusy = false; // Nueva bandera

    private void Start()
    {
        originalPosition = transform.position;

        if (geyserParticles == null)
            geyserParticles = GetComponentInChildren<ParticleSystem>();

        // Inicia el shake suave siempre activo
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
        if (other.CompareTag("Player") && geyserRoutine == null && !isBusy && Time.time - lastImpulseTime > cooldown)
        {
            geyserRoutine = StartCoroutine(GeyserSequence(other));
        }
    }

    private IEnumerator IdleShakeRoutine()
    {
        while (true)
        {
            // Solo agita si NO está en ciclo de impulso
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
        float shakeEndTime = Time.time + shakeDuration;

        if (geyserParticles != null)
            geyserParticles.Play();

        // Shake intenso mientras espera el impulso
        while (Time.time < shakeEndTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }
        transform.position = originalPosition;
        isShaking = false;

        yield return new WaitForSeconds(0.2f);

        // --- VERIFICACIÓN: ¿El jugador sigue sobre la plataforma? ---
        if (!playersOnPlatform.Contains(playerCollider))
        {
            if (geyserParticles != null)
                geyserParticles.Stop();
            geyserRoutine = null;
            isBusy = false;
            yield break;
        }

        // Subida
        float elapsed = 0f;
        Vector3 startPos = originalPosition;
        Vector3 endPos = originalPosition + Vector3.up * liftHeight;

        PlayerController player = playerCollider.GetComponent<PlayerController>();
        while (elapsed < liftDuration)
        {
            float t = elapsed / liftDuration;
            float explosionCurve = Mathf.Sin(t * Mathf.PI * explosionCurvePower);
            transform.position = Vector3.Lerp(startPos, endPos, explosionCurve);

            if (player != null && playersOnPlatform.Contains(playerCollider))
            {
                float impulse = Mathf.Lerp(geyserForce, geyserForce * impulseMinFactor, t);
                player.Velocity = new Vector3(player.Velocity.x, impulse, player.Velocity.z);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        lastImpulseTime = Time.time;
        yield return new WaitForSeconds(1f);

        // Caída
        float fallDuration = liftDuration * fallDurationFactor;
        elapsed = 0f;
        while (elapsed < fallDuration)
        {
            float t = elapsed / fallDuration;
            float fallCurve = Mathf.Pow(t, fallCurvePower);
            transform.position = Vector3.Lerp(endPos, originalPosition, fallCurve);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Asegura posición final

        if (geyserParticles != null)
            geyserParticles.Stop();

        geyserRoutine = null;
        isBusy = false;
    }
} */

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

        // Asignar al jugador como hijo de la plataforma
        Transform playerTransform = playerCollider.transform;
        playerTransform.SetParent(transform);
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
            playerTransform.SetParent(null);

        // Caída suave
        if ((transform.position - originalPosition).sqrMagnitude > 0.001f)
        {
            float fallDuration = liftDuration * fallDurationFactor;
            elapsed = 0f;

            while (elapsed < fallDuration)
            {
                float t = elapsed / fallDuration;
                float fallCurve = Mathf.Pow(t, fallCurvePower);
                transform.position = Vector3.Lerp(endPos, originalPosition, fallCurve);
                elapsed += Time.deltaTime;
                yield return null;
            }
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
}
