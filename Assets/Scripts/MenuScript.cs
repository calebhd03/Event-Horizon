using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject creditsScreen;
    public GameObject settingsScreen;
    public GameObject extrasScreen;

    //The three functions here open their respective menus and close out the main
    public void OpenSettings()
        {
            settingsScreen.SetActive(true);
            menuScreen.SetActive(false);
        }
    public void OpenCredits()
        {
            creditsScreen.SetActive(true);
            menuScreen.SetActive(false);
        }
    public void OpenExtras()
        {
            extrasScreen.SetActive(true);
            menuScreen.SetActive(false);
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

    //Exits from Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
