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
        PlayerData data = SaveSystem.LoadPlayerData();

        _controller = GetComponent<CharacterController>();
        _controller.enabled = false;

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

        _controller.enabled = true;
    }

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData(this);
        SaveSystem.SavePlayerData(playerData);

        TestValue();
    }
    
    public void TestValue()
    {
        testData += 1;
        Debug.Log(testData);
}
}