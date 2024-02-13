using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using StarterAssets;

public class DeathScreen : MonoBehaviour
{
    SaveSystemTest saveSystemTest;
    Progress progressScript; 
    private StarterAssetsInputs starterAssetsInputs;
    public GameObject Player;
    void Awake()
    {
        gameObject.SetActive(false);
        starterAssetsInputs = Player.GetComponent<StarterAssetsInputs>();
    }
    public void Replay()
    {
        ThirdPersonController TPC = FindObjectOfType<ThirdPersonController>();
        gameObject.SetActive(false);
        starterAssetsInputs.LoadInput(true);
        TPC.deathbool = false;
        TPC.LockCameraPosition = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }
}



