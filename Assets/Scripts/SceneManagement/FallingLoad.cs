using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingLoad : MonoBehaviour
{
    public GameObject loadingScreen;
    private void OnEnable()
    {
        StartCoroutine(LoadSceneAsync("The Center"));
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
