using UnityEngine;
using Unity.Cinemachine;

public class PaneoCameraSwitcher : MonoBehaviour
{
    public CinemachineCamera paneoVirtualCamera;
    public CinemachineCamera mainVirtualCamera;

    public void ReturnToMainCamera()
    {
        paneoVirtualCamera.Priority = 10;
        mainVirtualCamera.Priority = 20;
    }
}