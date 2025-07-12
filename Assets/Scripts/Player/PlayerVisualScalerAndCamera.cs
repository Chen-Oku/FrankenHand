using UnityEngine;
using Unity.Cinemachine;

public class PlayerVisualScalerAndCamera : MonoBehaviour
{
    public Transform visualModel; // Modelo visual del jugador
    public CinemachineSplineCart dollyCart; // Cart de la cámara
    public CinemachineCamera virtualCamera; // <-- Añade esta referencia
    public float minScale = 1f;
    public float maxScale = 2f;
    public float minDollyPos = 0f;
    public float maxDollyPos = 10f;
    public float minDistance = 5f;
    public float maxDistance = 20f;
    public float minFOV = 40f;
    public float maxFOV = 60f;

    private Transform cameraTransform;

    void Start()
    {
        if (dollyCart != null)
            cameraTransform = dollyCart.transform;
        else if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (visualModel == null || cameraTransform == null) return;

        float distance = Vector3.Distance(transform.position, cameraTransform.position);
        float t = 1f - Mathf.InverseLerp(minDistance, maxDistance, distance);

        // Escalado visual del jugador
        float scale = Mathf.Lerp(maxScale, minScale, t);
        visualModel.localScale = Vector3.one * scale;

        // Ajuste de la posición de la cámara en el spline
        if (dollyCart != null)
        {
            dollyCart.SplinePosition = Mathf.Lerp(minDollyPos, maxDollyPos, t);
        }

        // Ajuste del FOV de la CinemachineVirtualCamera
        if (virtualCamera != null)
        {
            float fov = Mathf.Lerp(minFOV, maxFOV, t);
            virtualCamera.Lens.FieldOfView = fov;
        }
    }
}