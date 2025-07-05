using UnityEngine;

public class SpeedyIcon : MonoBehaviour
{
    public float vibrateAmount = 0.1f; // Qué tanto vibra a los lados
    public float vibrateSpeed = 20f;   // Qué tan rápido vibra

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * vibrateSpeed) * vibrateAmount;
        transform.position = new Vector3(startPosition.x + offsetX, startPosition.y, startPosition.z);
    }
}
