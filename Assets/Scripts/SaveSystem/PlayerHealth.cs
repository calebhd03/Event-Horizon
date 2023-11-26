using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHealth
{
    public float currentGameHealth; // Holds the player's current health

    // Constructor with PlayerHealthMetric parameter
    public PlayerHealth(SaveSystemTest playerP)
    {
        currentGameHealth = playerP.currentGameHealth;
    }
}