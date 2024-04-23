using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class afterOutroLoad : MonoBehaviour
{
    public GameObject loadingScreen;

    private void OnEnable()
    {
        if (Background_Music.instance != null) Background_Music.instance.MenuMusic();
        StartCoroutine(LoadSceneAsync("Start Menu"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            loadingScreen.SetActive(true);
            yield return null;
        }
    }
}
