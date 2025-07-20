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

            if (playerController == null)
                playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.puedeMover = false;
                playerController.ForzarIdle();
            }

            // Inicia la corrutina para desactivar objetos antes de terminar el paneo
            StartCoroutine(DesactivarObjetosTrasDelay(3f)); // Cambia 3f por los segundos deseados
        }
    }

    private void Update()
    {
        if (paneoActivo && Input.GetKeyDown(KeyCode.Escape))
        {
            cameraSwitcher.ReturnToMainCamera();
            paneoActivo = false;
            if (playerController != null)
                playerController.puedeMover = true; // Devuelve el control al jugador
            DisableTrigger();
        }
    }

    private System.Collections.IEnumerator DesactivarObjetosTrasDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DesactivarObjetos();
        // El resto se hace en OnPaneoTerminado()
    }

    // Llama este método desde un Animation Event al final de la animación de paneo
    public void OnPaneoTerminado()
    {
        DesactivarObjetos(); // Apaga luces y partículas
        cameraSwitcher.ReturnToMainCamera(); // Vuelve a la cámara principal
        paneoActivo = false;
        if (playerController != null)
            playerController.puedeMover = true; // Devuelve el control al jugador
        DisableTrigger(); // Desactiva el trigger para que no se repita
    }

    // Llama este método desde un Animation Event al final de la animación
    public void DisableTrigger()
    {
        gameObject.SetActive(false);
    }

    public void DesactivarObjetos()
    {
        foreach (var obj in objetosADesactivar)
        {
            if (obj != null && obj != this.gameObject)
            {
                Debug.Log("Desactivando: " + obj.name);
                obj.SetActive(false);
            }
        }
    }
}