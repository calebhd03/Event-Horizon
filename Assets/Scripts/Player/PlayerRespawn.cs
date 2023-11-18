using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private HealthMetrics healthMetrics;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the HealthMetrics script
        healthMetrics = GetComponent<HealthMetrics>();

        // Subscribe to the OnHealthChanged event
        healthMetrics.OnHealthChanged += CheckForRespawn;
    }

    // Update is called once per frame
    void Update()
    {
        // You can add other respawn logic or checks here if needed
    }

    // Method to check for respawn when health is <= 0
    private void CheckForRespawn(float currentHealth, float maxHealth)
    {
        if (currentHealth <= 0f)
        {
            // Call the LoadPlayer method from SaveSystem when health is <= 0
            PlayerSaveData loadedPlayerData = SaveSystem.LoadPlayer();
            
            // Add logic to respawn the player using the loaded data (e.g., set position, reset health, etc.)
            RespawnPlayer(loadedPlayerData);
        }
    }

    // Example method for respawning the player with loaded data
    private void RespawnPlayer(PlayerSaveData loadedPlayerData)
    {
        // Implement your respawn logic here using the loaded player data
        // For example, set the player's position, reset health, etc.
        // You might need to modify this based on your game requirements.
        
        // For demonstration purposes, let's just log a message for now.
        Debug.Log("Player Respawned!");
    }
}
