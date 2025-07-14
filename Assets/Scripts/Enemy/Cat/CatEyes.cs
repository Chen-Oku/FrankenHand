using System.Collections;
using UnityEngine;

public class CatEyes : MonoBehaviour
{
    public float eyesInterval = 2f;
    public SpriteRenderer ojosSprite; // Asigna el SpriteRenderer en el inspector
    public Animator ojosAnimator;     // Asigna el Animator en el inspector

    private bool eyesCycleActive = false;
    private Coroutine eyesCoroutine;

    void Start()
    {
        HideEyes();
    }

    public void ShowEyes()
    {
        Debug.Log("ShowEyes called");
        if (ojosSprite != null)
            ojosSprite.enabled = true;
        if (ojosAnimator != null)
            ojosAnimator.SetTrigger("Show");
    }

    public void HideEyes()
    {
        Debug.Log("HideEyes called");
        if (ojosSprite != null)
            ojosSprite.enabled = false;
    }

    public void StartEyesCycle()
    {
        if (!eyesCycleActive)
        {
            eyesCycleActive = true;
            eyesCoroutine = StartCoroutine(EyesCycle());
        }
    }

    public void StopEyesCycle()
    {
        eyesCycleActive = false;
        if (eyesCoroutine != null)
        {
            StopCoroutine(eyesCoroutine);
            eyesCoroutine = null;
        }
        HideEyes();
    }

    private IEnumerator EyesCycle()
    {
        while (eyesCycleActive)
        {
            ShowEyes();
            yield return new WaitForSeconds(eyesInterval);
            HideEyes();
            yield return new WaitForSeconds(eyesInterval);
        }
    }
}