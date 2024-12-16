using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDelay : MonoBehaviour
{
    public CanvasGroup menuCanvasGroup;
    public float delayTime = 5f;
    public float fadeDuration = 1f;

    void Start()
    {
        if(menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
            StartCoroutine(FadeInAfterDelay());
        }
    }

    IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            menuCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
    }
}
