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
    public int blackHoleAmmoSave;
    public int blackHoleAmmoLoadedSave;
    public int shotgunAmmoSave;
    public int shotgunAmmoLoadedSave;
    public int currentGameHealth;

    private CharacterController _controller;
    private ThirdPersonShooterController thirdPersonShooterController;
    private int lastSavedSceneIndex;

    // Reference to your PlayerHealthMetric script
    private PlayerHealthMetric playerHealthMetric;

    void Start()
    {
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _controller = GetComponent<CharacterController>();
        lastSavedSceneIndex = PlayerPrefs.GetInt("LastSavedSceneIndex", -1);

        // Get the PlayerHealthMetric script attached to the same GameObject
        playerHealthMetric = GetComponent<PlayerHealthMetric>();
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
            thirdPersonShooterController.standardAmmo = saveData.ammoData.standardAmmoSave;
            thirdPersonShooterController.standardAmmoLoaded = saveData.ammoData.standardAmmoLoadedSave;
            thirdPersonShooterController.blackHoleAmmo = saveData.ammoData.blackHoleAmmoSave;
            thirdPersonShooterController.blackHoleAmmoLoaded = saveData.ammoData.blackHoleAmmoLoadedSave;
            thirdPersonShooterController.shotgunAmmo = saveData.ammoData.shotgunAmmoSave;
            thirdPersonShooterController.shotgunAmmoLoaded = saveData.ammoData.shotgunAmmoLoadedSave;

            // Call the UpdateAmmoCount() function to update the UI or game logic
            thirdPersonShooterController.UpdateAmmoCount();

            // Access health data
            playerHealthMetric.currentHealth = saveData.healthData.currentGameHealth;

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
                    thirdPersonShooterController.standardAmmo = ammoData.standardAmmoSave;
                    thirdPersonShooterController.standardAmmoLoaded = ammoData.standardAmmoLoadedSave;
                    thirdPersonShooterController.blackHoleAmmo = ammoData.blackHoleAmmoSave;
                    thirdPersonShooterController.blackHoleAmmoLoaded = ammoData.blackHoleAmmoLoadedSave;
                    thirdPersonShooterController.shotgunAmmo = ammoData.shotgunAmmoSave;
                    thirdPersonShooterController.shotgunAmmoLoaded = ammoData.shotgunAmmoLoadedSave;
                    thirdPersonShooterController.UpdateAmmoCount();
                }
            }
        }
    }

    public void SaveGame()
    {
        // Save the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LastSavedSceneIndex", currentSceneIndex);

        // Create instances of PlayerData, PlayerAmmoData, and PlayerHealth
        PlayerData playerData = new PlayerData(this);
        PlayerAmmoData ammoData = new PlayerAmmoData(this);
        PlayerHealth healthData = new PlayerHealth(this);

        // Set the ammunition counts and health data to be saved
        ammoData.standardAmmoSave = thirdPersonShooterController.standardAmmo;
        ammoData.standardAmmoLoadedSave = thirdPersonShooterController.standardAmmoLoaded;
        ammoData.blackHoleAmmoSave = thirdPersonShooterController.blackHoleAmmo;
        ammoData.blackHoleAmmoLoadedSave = thirdPersonShooterController.blackHoleAmmoLoaded;
        ammoData.shotgunAmmoSave = thirdPersonShooterController.shotgunAmmo;
        ammoData.shotgunAmmoLoadedSave = thirdPersonShooterController.shotgunAmmoLoaded;
        healthData.currentGameHealth = playerHealthMetric.currentHealth;

        // Create a PlayerSaveData instance
        PlayerSaveData saveData = new PlayerSaveData(playerData, ammoData, healthData);

        // Save the data
        SaveSystem.SavePlayer(saveData);
    }

    public void TestValue()
    {
        testData += 1;
    }
}