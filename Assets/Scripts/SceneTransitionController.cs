using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image fadeImage;
    public float fadeDuration = 0.3f;

    private bool isFading = false;

    private void Start()
    {
        // Start with a fully opaque image
        fadeImage.color = Color.black;
        StartCoroutine(FadeOut());
    }

    public bool IsFading()
    {
        return isFading;
    }

    public IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
        isFading = false;
    }

    public IEnumerator FadeIn(string sceneName)
    {
        isFading = true;
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
        isFading = false;
        if(sceneName == "Inner")
        {
            Background_Music.instance.InnerMusic();
        }
        else if(sceneName == "The Center")
        {
            if (Background_Music.instance != null)
                {
                    Background_Music.instance.PauseMusic();
                }
        }
        else if (sceneName == "Start Menu")
        {
            Background_Music.instance.MenuMusic();
        }
        else if (sceneName == "OutroCutScene")
        {
            Background_Music.instance.audioSource.Stop();
        }
        // Load the next scene after fading in
         StartCoroutine(LoadSceneAsync(sceneName));
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
           if(loadingScreen != null)
           {
                loadingScreen.SetActive(true);
           }

           yield return null;
        }
    }
}