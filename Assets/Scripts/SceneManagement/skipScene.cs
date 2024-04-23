using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class skipScene : MonoBehaviour
{
    public GameObject loadingScreen;

    public string skipSceneTo;
    public void NextScene()
    {
        //loadingScreen.SetActive(true);
        if (skipSceneTo == "TheOuterVer2")
        {
            if (Background_Music.instance != null) Background_Music.instance.OuterMusic();
        }
        else if (skipSceneTo == "Start Menu")
        {
            if (Background_Music.instance != null) Background_Music.instance.MenuMusic();
        }

        StartCoroutine(LoadSceneAsync(skipSceneTo));
    }
    public void Awake()
    {
        Cursor.visible = true;
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