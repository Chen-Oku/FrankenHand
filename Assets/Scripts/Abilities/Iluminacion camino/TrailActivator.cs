using UnityEngine;

public class TrailActivator : MonoBehaviour
{
    public LightPoint[] trailPoints; // Asignar en orden desde el inspector
    private bool trailActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !trailActive)
        {
            trailActive = true;
            ResetTrail();
            trailPoints[0].Activate(); // Comenzar la guía
        }
    }

    public void ResetTrail()
    {
        foreach (var point in trailPoints)
        {
            point.ResetState();
        }
    }

    public void AdvanceToNext(int currentIndex)
    {
        if (currentIndex + 1 < trailPoints.Length)
        {
            trailPoints[currentIndex + 1].Activate();
        }
        else
        {
            // Fin del camino: reiniciar
            trailActive = false;
            ResetTrail();
        }
    }
}
