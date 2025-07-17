using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class PaneoCamera : MonoBehaviour
{
    public CinemachineSplineDolly splineDolly;
    public CinemachineSplineCart dollyCart; // El cart que mueve la cámara
    public float[] paneoPositions; // Ejemplo: [2.5f, 7.0f]
    public float paneoSpeed = 1f;

    private float originalPosition;
    private bool isPaneoActive = false;

    public CinemachineCamera paneoVirtualCamera;
    public CinemachineCamera mainVirtualCamera;

    public void StartPaneo()
    {
        if (!isPaneoActive)
        {
            paneoVirtualCamera.Priority = 20; // Más alta que la principal
            mainVirtualCamera.Priority = 10;
            StartCoroutine(PaneoSequence());
        }
        splineDolly.AutomaticDolly.Enabled = false;

    }

    private IEnumerator PaneoSequence()
    {
        isPaneoActive = true;
        originalPosition = dollyCart.SplinePosition;// Guarda la posición original del dolly cart

        // 1. Paneo hacia la pared
        yield return MoveDollyTo(paneoPositions[0]);

        // 2. Barrido hacia el objeto
        yield return MoveDollyTo(paneoPositions[1]);

        // 3. Retorna al jugador (posición original)
        yield return MoveDollyTo(originalPosition);

        // Al terminar el paneo:
        paneoVirtualCamera.Priority = 10;
        mainVirtualCamera.Priority = 20;
        isPaneoActive = false;
    }

    private IEnumerator MoveDollyTo(float targetPosition)
    {
        while (Mathf.Abs(dollyCart.SplinePosition - targetPosition) > 0.01f)
        {
            dollyCart.SplinePosition = Mathf.MoveTowards(dollyCart.SplinePosition, targetPosition, paneoSpeed * Time.deltaTime);
            yield return null;
        }
        dollyCart.SplinePosition = targetPosition;

    }
}
