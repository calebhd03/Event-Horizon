using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemTest : MonoBehaviour
{
    public int testData = 1;
    
    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        testData = data.testData;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        Debug.Log(position);
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
