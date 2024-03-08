using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VSLoad : MonoBehaviour
{
    private void OnEnable()
    {
        Background_Music.instance.OuterMusic();
        SceneManager.LoadScene("TheOuterVer2", LoadSceneMode.Single);
    }
}
