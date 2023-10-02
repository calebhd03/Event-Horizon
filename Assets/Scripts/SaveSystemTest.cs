using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemTest : MonoBehaviour
{
    public int testData = 1;
    
    void Update()
    {
        if (Input.GetKeyDown("[")) //Add value to test variable
        {
            testData += 1;
            Debug.Log(testData);
        }
        if (Input.GetKeyDown("o")) //Debug input to test save
        {
            SaveGame();
        }

        if (Input.GetKeyDown("p")) //Debug input to test loading
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

        if(Input.GetKeyDown("]"))
        {
            //Debug.Log("Player position before change:" + transform.position);
            transform.position = new Vector3(0,0,0);
            //Debug.Log("Player Moved to (0, 0, 0)");
            //Debug.Log("Player position after change:" + transform.position);
        }
    }

    public void SaveGame()
    {
        SaveSystem.SavePlayer(this);
    }
}
