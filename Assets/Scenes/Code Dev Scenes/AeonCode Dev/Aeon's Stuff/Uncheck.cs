using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uncheck : MonoBehaviour
{
    public PlayerData playerData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set both PickUp1 and PickUp2 to false
            playerData.PickUp1 = false;
            playerData.PickUp2 = false;

            // Debug message (optional)
            Debug.Log("PickUp1 and PickUp2 set to false.");

            // Destroy the game object (optional)
            Destroy(gameObject);
        }
    }
}