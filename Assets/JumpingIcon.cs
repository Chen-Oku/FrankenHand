using UnityEngine;

public class JumpingIcon : MonoBehaviour
{
    public float jumpHeight = 0.5f; // Altura máxima del salto
    public float jumpSpeed = 2f;    // Velocidad del salto

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Abs(Mathf.Sin(Time.time * jumpSpeed)) * jumpHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
