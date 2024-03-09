using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingScene : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("The Center", LoadSceneMode.Single);
    }
}
