using UnityEngine;

public class PlayerVisualScaler : MonoBehaviour
{
    public Transform visualModel;      // Asigna aquí el modelo visual del personaje
    public Transform cameraTransform;  // Se asigna automáticamente si está vacío
    public float minScale = 1f;
    public float maxScale = 2f;
    public float minDistance = 5f;
    public float maxDistance = 20f;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (visualModel == null || cameraTransform == null) return;

        float distance = Vector3.Distance(transform.position, cameraTransform.position);
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float scale = Mathf.Lerp(maxScale, minScale, t);
        visualModel.localScale = Vector3.one * scale;
    }
}