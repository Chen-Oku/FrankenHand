using UnityEngine;

public class TriggerPaw : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var paw = GetComponentInParent<PawAttk>();
            if (paw != null)
            {
                paw.TryAttack(); // Aplica el daño
            }
        }
    }
}