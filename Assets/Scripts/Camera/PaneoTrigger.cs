using UnityEngine;

public class PaneoTrigger : MonoBehaviour
{
    public PaneoCameraSwitcher cameraSwitcher;
    public Animator paneoAnimator;

    public GameObject[] objetosADesactivar; // Asigna aquí tus partículas y luces

    private bool paneoActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            paneoActivo = true;
            cameraSwitcher.paneoVirtualCamera.Priority = 20;
            cameraSwitcher.mainVirtualCamera.Priority = 10;
            cameraSwitcher.RestartPaneoAnimation();

            // Desactiva partículas y luces
            foreach (var obj in objetosADesactivar)
                if (obj != null) obj.SetActive(false);
        }
    }

    private void Update()
    {
        if (paneoActivo && Input.GetKeyDown(KeyCode.Escape))
        {
            cameraSwitcher.ReturnToMainCamera();
            paneoActivo = false;
            DisableTrigger();
        }
    }

    // Llama este método desde un Animation Event al final de la animación
    public void DisableTrigger()
    {
        gameObject.SetActive(false);
    }
}