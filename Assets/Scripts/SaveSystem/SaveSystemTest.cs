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
    public int blackHoleAmmoSave;
    public int shotgunAmmoSave;

    private CharacterController _controller;
    private ThirdPersonShooterController thirdPersonShooterController;
    private int lastSavedSceneIndex;

    void Start()
    {
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _controller = GetComponent<CharacterController>();
        lastSavedSceneIndex = PlayerPrefs.GetInt("LastSavedSceneIndex", -1);
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
            thirdPersonShooterController.blackHoleAmmo = saveData.ammoData.blackHoleAmmoSave;
            thirdPersonShooterController.shotgunAmmo = saveData.ammoData.shotgunAmmoSave;

            // Call the UpdateAmmoCount() function to update the UI or game logic
            thirdPersonShooterController.UpdateAmmoCount();

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
                    thirdPersonShooterController.blackHoleAmmo = ammoData.blackHoleAmmoSave;
                    thirdPersonShooterController.shotgunAmmo = ammoData.shotgunAmmoSave;
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

        if (lastSavedSceneIndex != currentSceneIndex)
        {
            // Scene index has changed, so save only PlayerAmmoData
            PlayerAmmoData ammoData = new PlayerAmmoData(this);
            // Set the ammunition counts to be saved
            ammoData.standardAmmoSave = thirdPersonShooterController.standardAmmo;
            ammoData.blackHoleAmmoSave = thirdPersonShooterController.blackHoleAmmo;
            ammoData.shotgunAmmoSave = thirdPersonShooterController.shotgunAmmo;
            SaveSystem.SavePlayerAmmoData(ammoData);
        }
        else
        {
            // Scene index has not changed, so save both PlayerData and PlayerAmmoData
            PlayerData playerData = new PlayerData(this);
            SaveSystem.SavePlayerData(playerData);

            PlayerAmmoData ammoData = new PlayerAmmoData(this);
            // Set the ammunition counts to be saved
            ammoData.standardAmmoSave = thirdPersonShooterController.standardAmmo;
            ammoData.blackHoleAmmoSave = thirdPersonShooterController.blackHoleAmmo;
            ammoData.shotgunAmmoSave = thirdPersonShooterController.shotgunAmmo;
            SaveSystem.SavePlayerAmmoData(ammoData);
        }
    }

    public void TestValue()
    {
        testData += 1;
        Debug.Log(testData);
    }
}