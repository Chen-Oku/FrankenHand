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
    public int lastSpawnIndex = -1;

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
}
