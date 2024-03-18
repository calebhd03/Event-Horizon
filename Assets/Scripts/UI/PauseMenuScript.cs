using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject PauseScreen;
    public GameObject settingsScreen;
    public GameObject inventoryScreen;
    public GameObject upgradeScreen;
    //public GameObject logPage;
    public GameObject HUD;
    
    public bool paused = false;

    private StarterAssetsInputs starterAssetsInputs;
    LogSystem logSystem;

    public GameObject Player;
    public bool dialogActive = false;



    private void Start()
    {
        logSystem = FindObjectOfType<LogSystem>();
        starterAssetsInputs = Player.GetComponent<StarterAssetsInputs>();
    }
    public void SetSave()
    {
        ClosePause();
        //starterAssetsInputs.PauseInput(false);
        starterAssetsInputs.SaveInput(true);
        paused = false;
        Invoke("DelayShoot", 0.1f);
    }
    public void SetLoad()
    {
        ClosePause();
        //starterAssetsInputs.PauseInput(false);
        starterAssetsInputs.LoadInput(true);
        paused = false;
        Invoke("DelayShoot", 0.1f);
    }

    public void SetPause()
    {
        if(paused == false)
        {
            OpenPause();
        }
        else
        {
            UnPause();
        }
    }
    //The three functions here open their respective menus and close out the main
    public void OpenPause()
    {
        PauseScreen.SetActive(true);
        PauseGame();
    }

    public void PauseGame()
    {
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        HUD.SetActive(false);
        starterAssetsInputs.delayShoot = true;
    }

    public void UnPause()
    {
        paused = false;
        settingsScreen.SetActive(false);
        inventoryScreen.SetActive(false);
        upgradeScreen.SetActive(false);
        ClosePause();
        Invoke("DelayShoot", 0.1f);
    }
    public void ClosePause()
    {
       // Debug.Log("Before: " + starterAssetsInputs.pause);
        
        settingsScreen.SetActive(false);
        inventoryScreen.SetActive(false);
        upgradeScreen.SetActive(false);

        paused = false;
        HUD.SetActive(true);
        PauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (dialogActive == false)
        {
        Time.timeScale = 1;
        }

        starterAssetsInputs.PauseInput(false);
        PauseFalse();


        //Debug.Log("After: " + starterAssetsInputs.pause);
    }
    public void OpenSettings()
        {
            PauseScreen.SetActive(false);
            settingsScreen.SetActive(true);
        }
    public void OpenInventory()
        {
            PauseScreen.SetActive(false);
            inventoryScreen.SetActive(true);
        }
    public void OpenUpgrades()
        {
            PauseScreen.SetActive(false);
            upgradeScreen.SetActive(true);
        }

    //add log system
    public void OpenLogSystem()
    {
        
        paused = false;

        PauseScreen.SetActive(false);
        if (logSystem.log == false)
        {
            starterAssetsInputs.LogInput(true);
        }
    }

        
    //The next three functions all close our their respective menus and return to Main
    public void CloseSettings()
        {
            settingsScreen.SetActive(false);
            PauseScreen.SetActive(true);
        }
    public void CloseInventory()
    {
        inventoryScreen.SetActive(false);
        PauseScreen.SetActive(true);
    }
    public void CloseUpgrades()
    {
        upgradeScreen.SetActive(false);
        PauseScreen.SetActive(true);
    }
    //add log system
    /*public void CloseLogSystem()
    {
        logPage.SetActive(false);
        PauseScreen.SetActive(true);
    }*/

    public void returnPause()
    {
        PauseScreen.SetActive(true);
    }

    //Exits from Game
    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void PauseFalse()
    {
        starterAssetsInputs.PauseInput(false);
        Invoke("DelayShoot", 0.1f);
    }

    public void DelayShoot()
    {
        starterAssetsInputs.delayShoot = false;
    }
}