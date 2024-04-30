using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;


public class StartMenu : MonoBehaviour
{
    private bool menuOpen = false;

    public GameObject start;
    public GameObject menu;
    public GameObject startButton;
    public GameObject VSSceneButton;
    public GameObject InnerSceneButton;
    public PlayerData playerData;
   
    void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        Cursor.visible = true;
        start.SetActive(true);
        menu.SetActive(false);
        string path = Application.persistentDataPath + "/playerData.data";
    }

    void Update()
    {
        if (Input.anyKey && menuOpen == false)
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
        // Call ResetHealthAmmo() from PlayerData script before loading the scene
        playerData.ResetHealthAmmo();
        playerData.ResetLogSystem();

        if (Background_Music.instance != null) Background_Music.instance.audioSource.Stop();
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
        if (obj == null)
        {
            Debug.LogError("Set selected obj is null");
            return;
        }
        EventSystem.current.SetSelectedGameObject(obj);
    }

    public void LoadTheOuterVer2Scene()
    {
        if (Background_Music.instance != null) Background_Music.instance.OuterMusic();
        Cursor.visible = false;
        SceneManager.LoadScene("TheOuterVer2");
    }

    public void LoadInnerScene()
    {
        if (Background_Music.instance != null) Background_Music.instance.InnerMusic();
        playerData.tutorialComplete = true;
        Cursor.visible = false;
        SceneManager.LoadScene("Inner");
    }

    public void LoadCenterScene()
    {
        if (Background_Music.instance != null) Background_Music.instance.CenterMusic();
        playerData.tutorialComplete = true;
        Cursor.visible = false;
        SceneManager.LoadScene("The Center");

    }
}