using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class skipScene : MonoBehaviour
{
    public string skipSceneTo;
    public void NextScene()
    {
        SceneManager.LoadScene(skipSceneTo);
    }
}