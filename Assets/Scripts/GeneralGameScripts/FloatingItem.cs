using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float amplitude = 0.25f; // Altura del movimiento
    public float frequency = 1f;    // Velocidad del movimiento

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Movimiento vertical tipo seno
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
        // Rotación suave
        transform.Rotate(Vector3.up, 40f * Time.deltaTime);
    }
}