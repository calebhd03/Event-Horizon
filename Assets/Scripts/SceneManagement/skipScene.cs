using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class skipScene : MonoBehaviour
{
    public string skipSceneTo;
    public void NextScene()
    {
        SceneManager.LoadScene(skipSceneTo);
    }
    public void Awake()
    {
        Cursor.visible = true;
    }
}