using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitch : MonoBehaviour
{
    public string sceneToLoad;  
    public float fadeDuration = 1f;  
    private ScreenFader screenFader;

    void Start()
    {
        screenFader = FindObjectOfType<ScreenFader>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SwitchScene());
        }
    }

    private IEnumerator SwitchScene()
    {
        if (screenFader != null)
        {
            yield return StartCoroutine(screenFader.FadeOut(fadeDuration));
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        if (screenFader != null)
        {
            yield return StartCoroutine(screenFader.FadeIn(fadeDuration));
        }
    }
}



