using UnityEngine;

public class PaneoTrigger : MonoBehaviour
{
    public PlayerController playerController; // Asigna en el inspector
    public PaneoCameraSwitcher cameraSwitcher;
    public Animator paneoAnimator;

    public GameObject[] objetosADesactivar; // Asigna aquí tus partículas y luces
    public float tiempoDesactivacion = 10f; // Tiempo en segundos para desactivar objetos

    private bool paneoActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            paneoActivo = true;
            cameraSwitcher.paneoVirtualCamera.Priority = 20;
            cameraSwitcher.mainVirtualCamera.Priority = 10;
            cameraSwitcher.RestartPaneoAnimation();

            // Asigna automáticamente el PlayerController si no está asignado
            if (playerController == null)
                playerController = other.GetComponent<PlayerController>();

            // Desactiva el movimiento del jugador
            if (playerController != null)
            {
                var movement = playerController.GetComponent<PlayerMovement>();
                if (movement != null)
                    movement.SetCanMove(false);
                playerController.ForzarIdle();
            }

            StartCoroutine(DesactivarObjetosTrasDelay(tiempoDesactivacion));
        }
    }

    private void Update()
    {
        if (paneoActivo && Input.GetKeyDown(KeyCode.Escape))
        {
            cameraSwitcher.ReturnToMainCamera();
            paneoActivo = false;
            if (playerController != null)
            {
                var movement = playerController.GetComponent<PlayerMovement>();
                if (movement != null)
                    movement.SetCanMove(true);
            } // Devuelve el control al jugador
            DisableTrigger();
        }
    }

    // Llama este método desde un Animation Event al final de la animación
    public void DisableTrigger()
    {
        gameObject.SetActive(false);
    }

    public void DesactivarObjetos()
    {
        foreach (var obj in objetosADesactivar)
            if (obj != null) obj.SetActive(false);
    }

    private System.Collections.IEnumerator DesactivarObjetosTrasDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DesactivarObjetos();
        cameraSwitcher.ReturnToMainCamera(); // Vuelve a la cámara principal
        paneoActivo = false;
        if (playerController != null)
        {
            var movement = playerController.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.SetCanMove(true); // Devuelve el control al jugador
        }
        DisableTrigger();
    }

}