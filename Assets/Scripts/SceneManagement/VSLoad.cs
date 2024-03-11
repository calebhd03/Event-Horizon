using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VSLoad : MonoBehaviour
{
    public GameObject loadingScreen;
    private void OnEnable()
    {
        Background_Music.instance.OuterMusic();
        StartCoroutine(LoadSceneAsync("TheOuterVer2"));
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
