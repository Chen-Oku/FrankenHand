using UnityEngine;
using Unity.Cinemachine;

public class PaneoCameraSwitcher : MonoBehaviour
{
    public CinemachineCamera paneoVirtualCamera;
    public CinemachineCamera mainVirtualCamera;
    public Animator paneoAnimator; // Asigna el Animator de la cámara de paneo

    private void Start()
    {
        if (paneoAnimator != null)
            paneoAnimator.enabled = false; // Desactiva el Animator al inicio
    }

    public void ReturnToMainCamera()
    {
        paneoVirtualCamera.Priority = 10;
        mainVirtualCamera.Priority = 20;
        if (paneoAnimator != null)
            paneoAnimator.enabled = false; // Desactiva el Animator al volver
    }

    public void RestartPaneoAnimation()
    {
        if (paneoAnimator != null)
        {
            paneoAnimator.enabled = true; // Activa el Animator
            paneoAnimator.Play(0, -1, 0f); // O usa el nombre del estado si prefieres
        }
    }
}