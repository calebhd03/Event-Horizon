using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
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

        // Load the next scene after fading in
        SceneManager.LoadScene(sceneName);
    }
}