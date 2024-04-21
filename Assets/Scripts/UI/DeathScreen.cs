using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using StarterAssets;

public class DeathScreen : MonoBehaviour
{    SaveSystemTest saveSystemTest;
    Progress progressScript; 
    private StarterAssetsInputs starterAssetsInputs;
    public GameObject Player;

    // Reference to the PlayerData scriptable object
    public PlayerData playerData;

    // Reference to the EnemyManager script
    private EnemyManager enemyManager;
    public GameObject HudObject;

    void Awake()
    {
        UnhideHud();
        gameObject.SetActive(false);
        starterAssetsInputs = Player.GetComponent<StarterAssetsInputs>();

        // Obtain a reference to the EnemyManager instance
        enemyManager = EnemyManager.instance;
        if (enemyManager == null)
            Debug.LogError("EnemyManager instance not found, ensure it's initialized before this script.");
    }
    public void OnEnable()
    {
        if(HudObject != null)
        {
        HudObject.SetActive(false);
        }
    }
    public void OnDisable()
    {
        if(HudObject != null)
        {
        HudObject.SetActive(true);
        }
    }
    void UnhideHud()
    {
        if(HudObject != null)
        {
        HudObject.SetActive(true);
        }
    }
    public void Replay()
    {
        // Obtain the active scene's index to pass to LoadEnemyLocations
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        ThirdPersonController TPC = FindObjectOfType<ThirdPersonController>();

        // Reset player health and ammo
        playerData.ResetSemiHealth();
        gameObject.SetActive(false);
        starterAssetsInputs.LoadInput(true);
        TPC.deathbool = false;
        TPC.LockCameraPosition = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Reload enemy positions using EnemyManager
        if (enemyManager != null)
        {
            enemyManager.LoadEnemyLocations(sceneIndex);
        }
        else
        {
            Debug.LogError("Failed to load enemy locations: EnemyManager is not available.");
        }
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