using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemTest : MonoBehaviour
{
    public int testData = 1;
    public int standardAmmoSave;
    public int blackHoleAmmoSave;
    public int shotgunAmmoSave;

    private CharacterController _controller;
    private ThirdPersonShooterController thirdPersonShooterController;

    void Start()
    {
        thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
    }
    
    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        _controller = GetComponent<CharacterController>(); 
        _controller.enabled = false;    //Disables Character Controller, fixes incorrect transform.position execution bug

        testData = data.testData;
        thirdPersonShooterController.standardAmmo = data.standardAmmoSave;
        thirdPersonShooterController.blackHoleAmmo = data.blackHoleAmmoSave;
        thirdPersonShooterController.shotgunAmmo = data.shotgunAmmoSave;
        thirdPersonShooterController.UpdateAmmoCount();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        Debug.Log(position);

        _controller.enabled = true; //Reenables Character Controller upon completion
    }

    public void SaveGame()
    {
        standardAmmoSave = thirdPersonShooterController.standardAmmo;
        blackHoleAmmoSave = thirdPersonShooterController.blackHoleAmmo;
        shotgunAmmoSave = thirdPersonShooterController.shotgunAmmo;
        SaveSystem.SavePlayer(this);
    }

    public void TestValue()
    {
        testData += 1;
    }
}
