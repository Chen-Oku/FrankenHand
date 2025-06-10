using UnityEngine;
using Unity.Cinemachine;

public class CameraSectionTrigger : MonoBehaviour
{
    public CinemachineCamera sectionCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger activado por el jugador");

            // Baja la prioridad de todas las cámaras
            foreach (var cam in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
            {
                cam.Priority = 0;
            }

            // Sube la prioridad de la cámara de esta sección
            if (sectionCamera != null)
            {
                sectionCamera.Priority = 10;
                Debug.Log("Cambiando a cámara: " + sectionCamera.name);
            }
            else
            {
                Debug.LogWarning("No se asignó una CinemachineCamera a este trigger.");
            }
        }
    }
}