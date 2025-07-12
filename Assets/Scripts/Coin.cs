using UnityEngine;

public class Coin : MonoBehaviour
{
    public int pointValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Asegúrate de que el jugador tenga el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Suma puntos al GameManager
            GameManager.Instance.AddPoints(pointValue);
            Debug.Log("Puntos sumados: " + pointValue);

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}
