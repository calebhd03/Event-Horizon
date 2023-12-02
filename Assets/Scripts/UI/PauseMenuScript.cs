using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;



public class PauseMenuScript : MonoBehaviour
{

    public GameObject PauseScreen;
    public GameObject settingsScreen;
    public GameObject inventoryScreen;
    public GameObject upgradeScreen;
    public GameObject logSystem;
    
    public bool paused = false;

    private StarterAssetsInputs starterAssetsInputs;

      public GameObject Player;



    private void Start()
    {
    
         starterAssetsInputs = Player.GetComponent<StarterAssetsInputs>();
    }
    public void SetSave()
    {
        ClosePause();
        starterAssetsInputs.PauseInput(false);
        starterAssetsInputs.SaveInput(true);
    }
    public void SetLoad()
    {
        ClosePause();
        starterAssetsInputs.PauseInput(false);
        starterAssetsInputs.LoadInput(true);
    }

    public void SetPause()
    {
        if(paused == false)
        {
           // paused = true;
            OpenPause();
        }
        else
        {
           // paused = false;
            settingsScreen.SetActive(false);
            inventoryScreen.SetActive(false);
            upgradeScreen.SetActive(false);
            logSystem.SetActive(false);
            ClosePause();
        }
    }
    //The three functions here open their respective menus and close out the main
    public void OpenPause()
        {
            paused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            PauseScreen.SetActive(true);

        }
    public void ClosePause()
    {
       // Debug.Log("Before: " + starterAssetsInputs.pause);
        
        settingsScreen.SetActive(false);
        inventoryScreen.SetActive(false);
        upgradeScreen.SetActive(false);
        logSystem.SetActive(false);

        paused = false;
        PauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

        starterAssetsInputs.PauseInput(false);



        //Debug.Log("After: " + starterAssetsInputs.pause);
    }
    public void OpenSettings()
        {
            settingsScreen.SetActive(true);
            PauseScreen.SetActive(false);
        }
    public void OpenInventory()
        {
            inventoryScreen.SetActive(true);
            PauseScreen.SetActive(false);
        }
    public void OpenUpgrades()
        {
            upgradeScreen.SetActive(true);
            PauseScreen.SetActive(false);
        }
    //add log system
    public void OpenLogSystem()
    {
        logSystem.SetActive(true);
        PauseScreen.SetActive(false);
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
    public void CloseLogSystem()
    {
        logSystem.SetActive(false);
        PauseScreen.SetActive(true);
    }

    public void returnPause()
    {
        PauseScreen.SetActive(true);
    }

    //Exits from Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
