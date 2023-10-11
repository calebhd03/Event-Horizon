using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemTest : MonoBehaviour
{
    public int testData = 1;
    private CharacterController _controller;
    
    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        _controller = GetComponent<CharacterController>(); 
        _controller.enabled = false;    //Disables Character Controller, fixes incorrect transform.position execution bug

        testData = data.testData;
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
        SaveSystem.SavePlayer(this);
    }

    public void TestValue()
    {
        testData += 1;
        Debug.Log(testData);
    }
}
