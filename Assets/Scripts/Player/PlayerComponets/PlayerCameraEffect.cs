using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerCameraEffect : MonoBehaviour
{
    public Transform cameraTransform;
    public CinemachineCamera virtualCamera; // Asigna en el inspector

    private CinemachineBasicMultiChannelPerlin perlin;

    void Awake()
    {
        if (virtualCamera != null)
            perlin = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
    }

    public void StartCameraShake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        if (cameraTransform == null) yield break;
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            cameraTransform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraTransform.localPosition = originalPos;
    }

    public void StartCinemachineShake(float duration, float amplitude)
    {
        if (perlin == null) return;
        StartCoroutine(CinemachineShakeCoroutine(duration, amplitude));
    }

    private IEnumerator CinemachineShakeCoroutine(float duration, float amplitude)
    {
        float originalAmplitude = perlin.AmplitudeGain;
        perlin.AmplitudeGain = amplitude;
        yield return new WaitForSeconds(duration);
        perlin.AmplitudeGain = originalAmplitude;
    }

    void LateUpdate()
    {
        // Billboarding: el objeto principal mira a la cámara (solo Y)
        if (cameraTransform != null)
        {
            Vector3 toCamera = cameraTransform.position - transform.position;
            toCamera.y = 0f;
            if (toCamera.sqrMagnitude > 1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
                transform.rotation = lookRotation;
            }
        }
    }
}