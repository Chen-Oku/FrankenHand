using UnityEngine;

public class LightPoint : MonoBehaviour
{
    public int indexInTrail;
    public TrailActivator trailController;

    public ParticleSystem particles;
    public Light lightSource; // Opcional: para iluminación real

    private bool isActive = false;

    void Start()
    {
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>(true);

        if (lightSource == null)
            lightSource = GetComponentInChildren<Light>(true);

        ResetState(); // Iniciar desactivado
    }

    public void Activate()
    {
        isActive = true;

        if (particles != null)
        {
            particles.gameObject.SetActive(true);
            particles.Play();
        }

        if (lightSource != null)
            lightSource.enabled = true;

        Debug.Log($"LightPoint {indexInTrail} activado");
    }

    public void ResetState()
    {
        isActive = false;

        if (particles != null)
        {
            particles.Stop();
            particles.Clear();
            particles.gameObject.SetActive(false);
        }

        if (lightSource != null)
            lightSource.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || !other.CompareTag("Player")) return;

        isActive = false;

        if (particles != null)
        {
            particles.Stop();
            particles.Clear();
            particles.gameObject.SetActive(false);
        }

        if (lightSource != null)
            lightSource.enabled = false;

        trailController.AdvanceToNext(indexInTrail);
    }
}
