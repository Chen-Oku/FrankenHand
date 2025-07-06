using UnityEngine;

public class BottlePlatform : MonoBehaviour
{
    private Transform playerOnPlatform;
    private Vector3 playerLocalOffset;
    private Quaternion playerLocalRotation;

    void Update()
    {
        if (playerOnPlatform != null)
        {
            // Mantén la posición relativa y la rotación del jugador respecto a la botella
            playerOnPlatform.position = transform.TransformPoint(playerLocalOffset);
            // Opcional: Si quieres que el jugador rote con la botella (solo si visualmente queda bien)
            // playerOnPlatform.rotation = transform.rotation * playerLocalRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = collision.transform;
            // Guarda la posición y rotación relativa al subirse
            playerLocalOffset = transform.InverseTransformPoint(playerOnPlatform.position);
            playerLocalRotation = Quaternion.Inverse(transform.rotation) * playerOnPlatform.rotation;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}