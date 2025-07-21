using UnityEngine;

public class PaneoTrigger : MonoBehaviour
{
    public PlayerController playerController;
    public PaneoCameraSwitcher cameraSwitcher;
    public Animator paneoAnimator;

    public GameObject[] objetosADesactivar;
    public float tiempoDesactivacion = 10f;

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
                playerController.puedeMover = true;
            DisableTrigger();
        }
    }

    private System.Collections.IEnumerator DesactivarObjetosTrasDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DesactivarObjetos();
    }


    public void OnPaneoTerminado()
    {
        DesactivarObjetos(); 
        cameraSwitcher.ReturnToMainCamera();
        paneoActivo = false;
        if (playerController != null)
            playerController.puedeMover = true;
        DisableTrigger(); 
    }

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