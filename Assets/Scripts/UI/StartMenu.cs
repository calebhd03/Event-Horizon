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
    public PlayerData playerData;

    void Start()
    {
        Cursor.visible = true;
        start.SetActive(true);
        menu.SetActive(false);
    }
    void Update()
    {
        if (Input.anyKey && menuOpen == false)
        {
            start.SetActive(false);
            menu.SetActive(true);
            menuOpen = true;
            //SetSelected(startButton);
        }

        string[] joystickNames = Input.GetJoystickNames();
        bool isControllerConnected = false;

        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(joystickNames[i]))
            {
                isControllerConnected = true;
                break;
            }
        }

        while (isControllerConnected)
        {
            SetSelected(startButton); // Activate the button
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

    public void SetSelected(GameObject obj)
    {
        if(obj == null)
        {
            Debug.LogError("Set selected obj is null");
            return;
        }
        EventSystem.current.SetSelectedGameObject(obj);
    }
     public void LoadTheOuterVer2Scene()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("TheOuterVer2");
    }
    public void LoadInnerScene()
    {
        playerData.tutorialComplete = true;
        Cursor.visible = false;
        SceneManager.LoadScene("Inner");
    }
    public void LoadCenterScene()
    {
        playerData.tutorialComplete = true;
        Cursor.visible = false;
        SceneManager.LoadScene("The Center");
        
    }
}
