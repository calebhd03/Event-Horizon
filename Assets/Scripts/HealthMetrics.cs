using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMetrics : MonoBehaviour
{
    // Health-related variables
    public float maxHealth = 100f; // Maximum health points
    public float currentHealth;    // Current health points

    // Events for health changes
    public delegate void HealthChangeAction(float currentHealth, float maxHealth);
    public event HealthChangeAction OnHealthChanged;

    //Debug visual health bar
    private playerHealthBarTest playerHealthBar;

    private void Awake()
    {
        //test
        playerHealthBar = GetComponent<playerHealthBarTest>();
    }
    // Initialization
    private void Start()
    {
         //currentHealth = maxHealth; // Initialize current health to max health

        //test
        playerHealthBar.updateHealthBar(currentHealth, maxHealth);
    }

    // Modify health points
    public void ModifyHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);

        playerHealthBar.updateHealthBar(currentHealth, maxHealth);

        // Trigger the event
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // Check for death
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Method to handle character death
    private void Die()
    {
        // Implement what should happen when the character dies
        // For example, you can destroy the game object, play death animations, etc.
        // You can override this method in derived classes for custom behavior.
        Destroy(gameObject);
    }

    // Usage:
// 1. Attach this script to a game object or character in your Unity scene.
// 2. In the Inspector, you can set the "maxHealth" variable to determine the maximum health points for the object.
// 3. Use the "ModifyHealth(float amount)" method to adjust health points (positive for healing, negative for damage).
// 4. Listen to the "OnHealthChanged" event to respond to health changes in your game code.
// 5. Customize the "Die()" method to handle character death, such as destroying the object or triggering death animations.
}
