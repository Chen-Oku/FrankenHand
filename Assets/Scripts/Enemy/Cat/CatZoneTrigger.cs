using UnityEngine;
using System.Collections.Generic;

public class CatZoneTrigger : MonoBehaviour
{
    public List<PawAttk> patas;
    public CatEyes catEyes;

    private void Start()
    {
        HashSet<int> occupiedIndices = new HashSet<int>();
        foreach (var pata in patas)
        {
            int idx = pata.AppearAtRandomPoint(occupiedIndices);
            occupiedIndices.Add(idx);
        }
        catEyes.HideEyes(); // Asegura que los ojos estén ocultos al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catEyes.MoveEyesToVisible();
            catEyes.ShowEyes();
            catEyes.StartEyesCycle();
            if (catEyes.ojosSprite != null)
                catEyes.ojosSprite.enabled = true;

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
        Debug.Log("Player entered the cat zone trigger.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catEyes.MoveEyesToOculta(); // Puedes eliminar esta línea si usas HideEyes con duración
            catEyes.HideEyes(0.5f);     // Mueve y luego oculta el sprite
            catEyes.StopEyesCycle();

            if (catEyes.ojosSprite != null)
                catEyes.ojosSprite.enabled = false;
                Debug.Log("Cat eyes hidden.");
            
            foreach (var pata in patas)
            {
                pata.player = null;
                pata.playerInZone = false;
            }
        }
        Debug.Log("Player exited the cat zone trigger.");
    }
}