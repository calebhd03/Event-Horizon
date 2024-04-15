using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject creditsScreen;
    public GameObject settingsScreen;
    public GameObject extrasScreen;
    public GameObject dataScreen;
    public GameObject difficultyScreen;
    public Toggle normalToggle, hardToggle;
    public static bool normalMode = true;
    public static int normalState = 1;
    public static bool hardMode = false;
    public static int hardState = 0;

    void Awake()
    {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            normalToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(normalToggle); });
            hardToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(hardToggle); });
            
            
    }
    void Start()
    {
        if(hardMode == true)
        {
            hardState = 1;
            normalState = 0;
            hardToggle.interactable = false;
        }
        else
        {
            hardState = 0;
            normalState = 1;
            normalToggle.interactable = false;
        }
        normalToggle.isOn = normalState == 1;
        hardToggle.isOn = hardState == 1;
    }
    //The three functions here open their respective menus and close out the main
    public void OpenSettings()
        {
            menuScreen.SetActive(false);
            settingsScreen.SetActive(true);
        }
    public void OpenCredits()
        {
            menuScreen.SetActive(false);
            creditsScreen.SetActive(true);
        }
    public void OpenExtras()
        {            
            menuScreen.SetActive(false);
            extrasScreen.SetActive(true);
        }
    public void OpenSaves()
        {
            menuScreen.SetActive(false);
            dataScreen.SetActive(true);
        }

        
    //The next three functions all close our their respective menus and return to Main
    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
    public void CloseExtras()
    {
        extrasScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
    public void CloseSaves()
    {
        dataScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    //Exits from Game
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Difficulty()
    {
        menuScreen.SetActive(false);
        difficultyScreen.SetActive(true);
    }

    public void CloseDifficulty()
    {
        difficultyScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
    void ToggleValueChanged(Toggle toggle)
    {
        if (toggle == normalToggle)
        {
            if (toggle.isOn)
            {
                toggle.interactable = false;
                hardToggle.interactable = true;
                normalMode = true;
                normalState = 1;

                hardState = 0;
                hardToggle.isOn = false;
                Debug.LogError("Your static bool value: " + hardMode);

            }
            else
            {
                normalMode = false;
                normalState = 0;
            }
        }
        else if (toggle == hardToggle)
        {
            if (toggle.isOn)
            {
                toggle.interactable = false;
                normalToggle.interactable = true;
                hardMode = true;
                hardState = 1;

                normalState = 0;
                normalToggle.isOn = false;
                Debug.LogError("Your static bool value: " + hardMode);

            }
            else
            {
                hardMode = false;
                hardState = 0;
            }
        }
    }
}
