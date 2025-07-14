using UnityEngine;
using System.Collections.Generic;

public class CatZoneTrigger : MonoBehaviour
{
    public List<PawAttk> patas; // Asigna todas las patas en el inspector (puedes usar un array también)
    public CatEyes catEyes;
    public SpriteRenderer ojosSprite;

    private void Start()
    {
        // Si quieres inicializar las patas en spawnPoints diferentes al inicio:
        HashSet<int> occupiedIndices = new HashSet<int>();
        foreach (var pata in patas)
        {
            int idx = pata.AppearAtRandomPoint(occupiedIndices);
            occupiedIndices.Add(idx);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catEyes.StartEyesCycle();

            foreach (var pata in patas)
            {
                pata.playerInZone = true;
                pata.player = other.transform;
                if (!pata.attackCycleActive)
                {
                    pata.attackCycleActive = true;
                    pata.StartCoroutine(pata.AttackCycle());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catEyes.StopEyesCycle();
            catEyes.HideEyes();

            if (ojosSprite != null)
            ojosSprite.enabled = false;

            foreach (var pata in patas)
            {
                pata.player = null;
                pata.playerInZone = false;
            }
        }
    }
}