using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemTest : MonoBehaviour
{
    public int testData = 1;
    
    void Update()
    {
        if (Input.GetKeyDown("["))
        {
            testData += 1;
            Debug.Log(testData);
        }
        if (Input.GetKeyDown("o"))
        {
            SaveSystem.SavePlayer(this);
            Debug.Log("Game Save input pressed.");
        }

        if (Input.GetKeyDown("p"))
        {
            PlayerData data = SaveSystem.LoadPlayer();

            testData = data.testData;
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            transform.position = position;
            Debug.Log(position);
            Debug.Log("Game Load input pressed.");
        }
    }
}
