using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private bool menuOpen = false;

    public GameObject start;
    public GameObject menu;

    void Start()
    {
        start.SetActive(true);
        menu.SetActive(false);
    }
    void Update()
    {
        if(Input.anyKey && menuOpen == false)
        {
            start.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void LoadCodePrototype()
    {
        SceneManager.LoadScene("CodePrototype");
    }

    public void LoadArtPrototype()
    {
        SceneManager.LoadScene("ArtPrototype");
    }

    public void LoadVerticalSlice()
    {
        SceneManager.LoadScene("VerticalSlice");
    }
}
