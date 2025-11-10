using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawAttk : MonoBehaviour
{
    public Animator pawAnimator;
    public float attackRange = 2f;
    public int damage = 1;
    public Transform[] spawnPoints;
    [HideInInspector] public bool playerInZone = false;
    [HideInInspector] public Transform player;
    [HideInInspector] public bool attackCycleActive = false;
    public float attackInterval = 2f;
    public SpriteRenderer pataSprite;
    public ParticleSystem attkDust;
    public int lastSpawnIndex = -1;

    // --- Trail renderer fields (nuevo) ---
    public TrailRenderer trailRenderer;          // assign in inspector
    public Transform trailStartMarker;           // marcador de inicio
    public Transform trailEndMarker;             // marcador de fin
    public float trailTravelDuration = 0.2f;     // tiempo para mover el trail de start a end (si > 0)
    public ParticleSystem sweepDust;            // partículas de polvo para barrido (asignar en inspector para la pata horizontal)
    public bool clearTrailImmediate = false;    // si true limpia el trail al terminar; si false permite que se desvanezca según trail.time
    private Coroutine trailMoveRoutine;
    private Coroutine endTrailRoutine;
    // ---------------------------------------

    void Start()
    {
        HidePaw();
    }

    public void ShowPaw()
    {
        if (pataSprite != null)
            pataSprite.enabled = true;
    }

    public void HidePaw()
    {
        if (pataSprite != null)
            pataSprite.enabled = false;
    }

    public int AppearAtRandomPoint(HashSet<int> occupiedIndices = null)
    {
        int idx;
        do
        {
            idx = Random.Range(0, spawnPoints.Length);
        } while ((occupiedIndices != null && occupiedIndices.Contains(idx)) || idx == lastSpawnIndex);

        transform.position = spawnPoints[idx].position;
        lastSpawnIndex = idx;
        return idx;
    }

    public void TryAttack()
    {
        // Reproduce el polvo siempre que se intente el ataque
        AttackDust();

        if (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            VidaPlayer vida = player.GetComponent<VidaPlayer>();
            if (vida != null)
            {
                vida.RecibirDanio(damage);

                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    Vector3 knockbackDir = (player.position - transform.position).normalized;
                    pc.ApplyKnockback(knockbackDir, 10f, 0.2f);
                    pc.FlashRed(0.5f, 3);

                    var sound = player.GetComponent<PlayerSoundController>();
                    if (sound != null)
                        sound.PlayBofetadaGatoRandom();
                }
            }
        }
    }

    public IEnumerator AttackCycle()
    {
        while (playerInZone)
        {
            AppearAtRandomPoint();
            ShowPaw();
            pawAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackInterval);
        }
        HidePaw();
        attackCycleActive = false;
    }

    void OnTriggerEnter(Collider other)
    {
        var paw = GetComponentInParent<PawAttk>();
        if (paw != null && other.CompareTag("Player"))
        {
            paw.playerInZone = true;
            paw.player = other.transform;
            paw.StartCoroutine(paw.AttackCycle());
        }
    }

    void AttackDust()
    {
        if (attkDust != null)
            attkDust.Play();
    }

    // --- Trail control methods para Animation Events ---
    // Sobrecarga para recibir el marcador desde el Animation Event (arrastra el GameObject en "Object")
    public void TrailStartEvent(GameObject startMarker)
    {
        if (startMarker != null)
            trailStartMarker = startMarker.transform;
        TrailStartEvent();
    }

    // Llamar desde el evento de animación al inicio del swing (sin parámetro)
    public void TrailStartEvent()
    {
        if (trailRenderer == null) return;

        // si hay una corutina de finalización en curso, cancelarla para que el trail no se borre mientras empieza otro swing
        if (endTrailRoutine != null)
        {
            StopCoroutine(endTrailRoutine);
            endTrailRoutine = null;
        }

        trailRenderer.Clear();
        if (trailStartMarker != null)
            trailRenderer.transform.position = trailStartMarker.position;

        trailRenderer.emitting = true;

        // particulas de polvo para barrido (usar en pata horizontal)
        if (sweepDust != null)
        {
            sweepDust.Play();
        }

        if (trailEndMarker != null && trailTravelDuration > 0f)
        {
            if (trailMoveRoutine != null) StopCoroutine(trailMoveRoutine);
            trailMoveRoutine = StartCoroutine(MoveTrail(trailTravelDuration));
        }
    }

    // Sobrecarga para recibir el marcador final desde el Animation Event
    public void TrailEndEvent(GameObject endMarker)
    {
        if (endMarker != null)
            trailEndMarker = endMarker.transform;
        TrailEndEvent();
        if (sweepDust != null)
        {
            sweepDust.Stop();
        }
    }

    // Llamar desde el evento de animación al final del swing (sin parámetro)
    public void TrailEndEvent()
    {
        if (trailRenderer == null) return;

        if (trailEndMarker != null)
            trailRenderer.transform.position = trailEndMarker.position;

        // detener partículas de barrido
        if (sweepDust != null)
        {
            sweepDust.Stop();
        }

        // detener el movimiento del trail si estaba en curso
        if (trailMoveRoutine != null)
        {
            StopCoroutine(trailMoveRoutine);
            trailMoveRoutine = null;
        }

        // si se quiere limpiar inmediato, hacerlo; si no, apagar emitting y esperar a que se desvanezca según trail.time
        if (endTrailRoutine != null)
        {
            StopCoroutine(endTrailRoutine);
            endTrailRoutine = null;
        }
        endTrailRoutine = StartCoroutine(EndTrailRoutine());
    }

    private IEnumerator EndTrailRoutine()
    {
        // Apagar emisión para que el trail deje de crear nuevas muestras
        trailRenderer.emitting = false;

        if (clearTrailImmediate)
        {
            trailRenderer.Clear();
        }
        else
        {
            // esperar el tiempo de vida del trail para que se desvanezca visualmente antes de limpiar
            float wait = Mathf.Max(0f, trailRenderer.time);
            if (wait > 0f)
                yield return new WaitForSeconds(wait);
            trailRenderer.Clear();
        }
        if (sweepDust != null)
        {
            sweepDust.Stop();
        }

        endTrailRoutine = null;
    }

    private IEnumerator MoveTrail(float duration)
    {
        if (trailStartMarker == null || trailEndMarker == null || trailRenderer == null) yield break;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / duration);
            trailRenderer.transform.position = Vector3.Lerp(trailStartMarker.position, trailEndMarker.position, f);
            yield return null;
        }

        trailRenderer.transform.position = trailEndMarker.position;
        // no forzar emit=false aquí; lo hace EndTrailRoutine para un comportamiento consistente
        trailMoveRoutine = null;
    }
    // ---------------------------------------
}
