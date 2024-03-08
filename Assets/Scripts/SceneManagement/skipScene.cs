using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class skipScene : MonoBehaviour
{
    public string skipSceneTo;
    public void NextScene()
    {
        if (skipSceneTo == "TheOuterVer2")
        {
            Background_Music.instance.OuterMusic();
        }
        else if (skipSceneTo == "Start Menu")
        {
            Background_Music.instance.MenuMusic();
        }
        SceneManager.LoadScene(skipSceneTo);
    }
    public void Awake()
    {
        Cursor.visible = true;
    }
}