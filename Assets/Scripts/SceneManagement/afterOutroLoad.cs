using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class afterOutroLoad : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Start Menu", LoadSceneMode.Single);
    }
}
