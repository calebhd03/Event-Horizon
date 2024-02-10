using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartMenu : MonoBehaviour
{
    private bool menuOpen = false;

    public GameObject start;
    public GameObject menu;
    public GameObject startButton;
    public GameObject VSSceneButton;
    public GameObject InnerSceneButton;

    void Start()
    {
        Cursor.visible = true;
        start.SetActive(true);
        menu.SetActive(false);
    }
    void Update()
    {
        if(Input.anyKey && menuOpen == false)
        {
            start.SetActive(false);
            menu.SetActive(true);
            menuOpen = true;
            SetSelected(startButton);
        }
    }

    public void LoadCodePrototype()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("CodePrototype");
    }

    public void LoadArtPrototype()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("ArtPrototype");
    }
    public void StartGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("IntroCutScene");
    }

    public void LoadVerticalSlice()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("VerticalSlice");
    }

    public void SetSelected(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void LoadInnerScene()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("Inner");
    }
    public void LoadCenterScene()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("The Center");
    }
}
