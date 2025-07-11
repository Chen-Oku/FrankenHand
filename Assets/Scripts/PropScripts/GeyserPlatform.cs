using System.Collections;
using UnityEngine;

public class GeyserPlatform : MonoBehaviour
{
    public float geyserForce = 15f;
    public float cooldown = 2f;
    public float shakeDuration = 1f;
    public float shakeAmount = 0.1f;
    public float liftDuration = 1f;
    public float liftHeight = 5f;

    public ParticleSystem geyserParticles;

    private float lastImpulseTime = -10f;
    private bool isShaking = false;
    private Vector3 originalPosition;
    private Coroutine geyserRoutine;

    private void Start()
    {
        originalPosition = transform.position;

        // Si no se asignó en el inspector, busca el primer ParticleSystem hijo
        if (geyserParticles == null)
            geyserParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && geyserRoutine == null && Time.time - lastImpulseTime > cooldown)
        {
            geyserRoutine = StartCoroutine(GeyserSequence(other));
        }
    }

    private IEnumerator GeyserSequence(Collider playerCollider)
    {
        isShaking = true;
        float shakeEndTime = Time.time + shakeDuration;

        // Inicia partículas de vapor justo al empezar a agitar
        if (geyserParticles != null)
            geyserParticles.Play();

        // Shake effect
        while (Time.time < shakeEndTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }
        transform.position = originalPosition;
        isShaking = false;

        // Espera antes de levantar
        yield return new WaitForSeconds(0.2f);

        // (Las partículas siguen durante el impulso)
        float elapsed = 0f;
        Vector3 startPos = originalPosition;
        Vector3 endPos = originalPosition + Vector3.up * liftHeight;

        PlayerController player = playerCollider.GetComponent<PlayerController>();
        while (elapsed < liftDuration)
        {
            float t = elapsed / liftDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (player != null)
            {
                player.Velocity = new Vector3(player.Velocity.x, geyserForce, player.Velocity.z);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;

        // Cooldown before next impulse
        lastImpulseTime = Time.time;
        yield return new WaitForSeconds(0.5f);

        // Return platform to original position
        elapsed = 0f;
        while (elapsed < liftDuration)
        {
            float t = elapsed / liftDuration;
            transform.position = Vector3.Lerp(endPos, originalPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        // Detén las partículas al terminar el ciclo
        if (geyserParticles != null)
            geyserParticles.Stop();

        geyserRoutine = null;
    }
}