using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveSystemTest : MonoBehaviour
{
    public int testData = 1;
    public int standardAmmoSave;
    public int standardAmmoLoadedSave;
    public int nexusAmmoSave;
    public int nexusAmmoLoadedSave;
    public int shotgunAmmoSave;
    public int shotgunAmmoLoadedSave;
    public int currentGameHealth;

    private CharacterController _controller;
    private ThirdPersonShooterController thirdPersonShooterController;
    private int lastSavedSceneIndex;

    // Reference to your PlayerHealthMetric script
    private PlayerHealthMetric playerHealthMetric;
    public PlayerData playerData;

    void Start()
    {
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _controller = GetComponent<CharacterController>();
        lastSavedSceneIndex = PlayerPrefs.GetInt("LastSavedSceneIndex", -1);

        // Get the PlayerHealthMetric script attached to the same GameObject
        playerHealthMetric = GetComponent<PlayerHealthMetric>();
        //playerData = GetComponent<PlayerData>();
    }

    public void LoadGame()
    {
        PlayerSaveData saveData = SaveSystem.LoadPlayer();

        if (saveData != null)
        {
            // Access player position data
            Vector3 playerPosition = new Vector3(saveData.playerData.position[0], saveData.playerData.position[1], saveData.playerData.position[2]);
            transform.position = playerPosition;

            // Access ammunition data
            testData = saveData.ammoData.testData;
            playerData.standardAmmo = saveData.ammoData.standardAmmoSave;
            playerData.standardAmmoLoaded = saveData.ammoData.standardAmmoLoadedSave;
            playerData.nexusAmmo = saveData.ammoData.nexusAmmoSave;
            playerData.nexusAmmoLoaded = saveData.ammoData.nexusAmmoLoadedSave;
            playerData.shotgunAmmo = saveData.ammoData.shotgunAmmoSave;
            playerData.shotgunAmmoLoaded = saveData.ammoData.shotgunAmmoLoadedSave;

            // Call the UpdateAmmoCount() function to update the UI or game logic
            thirdPersonShooterController.UpdateAmmoCount();

            // Access health data
            playerData.currentHealth = saveData.healthData.currentGameHealth;
            playerHealthMetric.UpdateHealthBar();

            // Check if the scene index has changed
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (lastSavedSceneIndex != currentSceneIndex)
            {
                // Scene index has changed, so load only PlayerAmmoData
                PlayerAmmoData ammoData = SaveSystem.LoadPlayerAmmoData();
                if (ammoData != null)
                {
                    // Set the ammunition counts
                    playerData.standardAmmo = ammoData.standardAmmoSave;
                    playerData.standardAmmoLoaded = ammoData.standardAmmoLoadedSave;
                    playerData.nexusAmmo = ammoData.nexusAmmoSave;
                    playerData.nexusAmmoLoaded = ammoData.nexusAmmoLoadedSave;
                    playerData.shotgunAmmo = ammoData.shotgunAmmoSave;
                    playerData.shotgunAmmoLoaded = ammoData.shotgunAmmoLoadedSave;
                    thirdPersonShooterController.UpdateAmmoCount();
                }
            }

            // Load the enemy data with the current scene index
            int sceneIndexToLoad = SceneManager.GetActiveScene().buildIndex;
            EnemyManager.instance.LoadEnemyLocations(sceneIndexToLoad);
        }
    }

    public void SaveGame()
    {
        // Save the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LastSavedSceneIndex", currentSceneIndex);

        // Create instances of PlayerData, PlayerAmmoData, and PlayerHealth
        PlayerDataOld playerDataOld = new PlayerDataOld(this);
        PlayerAmmoData ammoData = new PlayerAmmoData(this);
        PlayerHealth healthData = new PlayerHealth(this);

        // Set the ammunition counts and health data to be saved
        ammoData.standardAmmoSave = playerData.standardAmmo;
        ammoData.standardAmmoLoadedSave = playerData.standardAmmoLoaded;
        ammoData.nexusAmmoSave = playerData.nexusAmmo;
        ammoData.nexusAmmoLoadedSave = playerData.nexusAmmoLoaded;
        ammoData.shotgunAmmoSave = playerData.shotgunAmmo;
        ammoData.shotgunAmmoLoadedSave = playerData.shotgunAmmoLoaded;
        healthData.currentGameHealth = playerData.currentHealth;

        // Create a PlayerSaveData instance
        PlayerSaveData saveData = new PlayerSaveData(playerDataOld, ammoData, healthData);

        // Save the data
        SaveSystem.SavePlayer(saveData);

        // Find EnemyData object in the scene
        int sceneIndexToSave = SceneManager.GetActiveScene().buildIndex;
        EnemyManager.instance.SaveEnemyLocations(sceneIndexToSave);
    }

    public void TestValue()
    {
        testData += 1;
    }
}