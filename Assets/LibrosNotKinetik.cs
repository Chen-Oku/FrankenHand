using UnityEngine;

public class LibrosNotKinetik : MonoBehaviour
{
    public Rigidbody[] librosRigidbodies;
    public float delay = 0.2f;

    void Start()
    {
        StartCoroutine(ActivarFisicaDespuesDeDelay());
    }

    private System.Collections.IEnumerator ActivarFisicaDespuesDeDelay()
    {
        yield return new WaitForSeconds(delay);
        foreach (var rb in librosRigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
