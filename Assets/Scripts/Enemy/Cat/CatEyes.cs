using UnityEngine;
using System.Collections;

public class CatEyes : MonoBehaviour
{
    public SpriteRenderer ojosSprite;
    public Animator ojosAnimator;
    public Vector3 posicionVisible;
    public Vector3 posicionOculta;
    public float intervalo = 2f; // Intervalo entre apariciones

    private Coroutine eyesCoroutine;

    void Start()
    {
        // Al iniciar, mueve los ojos a la posición oculta y desactiva el sprite
        transform.localPosition = posicionOculta;
        if (ojosSprite != null)
            ojosSprite.enabled = false;
    }

    public void ShowEyes()
    {
        if (ojosSprite != null)
            ojosSprite.enabled = true;
        if (ojosAnimator != null)
            ojosAnimator.SetTrigger("Show");
    }

    public void HideEyes()
    {
        if (ojosSprite != null)
            ojosSprite.enabled = false;
        if (ojosAnimator != null)
            ojosAnimator.Play("IdleEyes");
    }

    public void HideEyes(float duration = 0.5f)
    {
        if (ojosAnimator != null)
            ojosAnimator.Play("IdleEyes");
        StartCoroutine(HideEyesRoutine(duration));
    }

    private IEnumerator HideEyesRoutine(float duration)
    {
        yield return StartCoroutine(MoveEyes(posicionOculta, duration));
        if (ojosSprite != null)
            ojosSprite.enabled = false;
    }

    public void StartEyesCycle()
    {
        if (eyesCoroutine == null)
            eyesCoroutine = StartCoroutine(EyesCycle());
    }

    public void StopEyesCycle()
    {
        if (eyesCoroutine != null)
        {
            StopCoroutine(eyesCoroutine);
            eyesCoroutine = null;
        }
        HideEyes();
    }

    private IEnumerator EyesCycle()
    {
        while (true)
        {
            ShowEyes();
            yield return new WaitForSeconds(intervalo);
            HideEyes();
            yield return new WaitForSeconds(intervalo);
        }
    }

    // Coroutine para mover suavemente los ojos
    private IEnumerator MoveEyes(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPos;
    }

    public void MoveEyesToVisible(float duration = 0.5f)
    {
        StartCoroutine(MoveEyes(posicionVisible, duration));
    }

    public void MoveEyesToOculta(float duration = 0.5f)
    {
        StartCoroutine(MoveEyes(posicionOculta, duration));
    }

    public void FadeInEyes(float duration = 1f)
    {
        if (ojosSprite != null)
        {
            ojosSprite.enabled = true;
            StartCoroutine(FadeEyesCoroutine(0f, 1f, duration));
        }
    }

    public void FadeOutEyes(float duration = 1f)
    {
        if (ojosSprite != null)
        {
            StartCoroutine(FadeEyesCoroutine(1f, 0f, duration));
        }
    }

    private IEnumerator FadeEyesCoroutine(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = ojosSprite.color;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            ojosSprite.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ojosSprite.color = new Color(color.r, color.g, color.b, to);
        ojosSprite.enabled = to > 0f;
    }
}