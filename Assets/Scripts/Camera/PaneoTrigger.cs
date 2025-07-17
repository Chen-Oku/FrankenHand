using UnityEngine;

public class PaneoTrigger : MonoBehaviour
{
    public PaneoCameraSwitcher cameraSwitcher;
    public Animator paneoAnimator;

    private bool paneoActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraSwitcher.paneoVirtualCamera.Priority = 20;
            cameraSwitcher.mainVirtualCamera.Priority = 10;
            paneoActivo = true;
        }
    }

    private void Update()
    {
        if (paneoActivo && Input.GetKeyDown(KeyCode.Escape))
        {
            cameraSwitcher.ReturnToMainCamera();
            paneoActivo = false;
        }
    }
}