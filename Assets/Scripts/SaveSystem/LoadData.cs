using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    private bool hasLoadedData = false;
   

    private void OnTriggerEnter(Collider other)
    {
        if (!hasLoadedData)
        {
            // Load the game data (assuming SaveSystemTest is attached to the player)
            SaveSystemTest saveSystem = FindObjectOfType<SaveSystemTest>();
            if (saveSystem != null)
            {
                saveSystem.LoadGame();

            }
            else
            {
                Debug.LogError("SaveSystemTest reference is not set on LoadData script.");
            }
        }
    }
}