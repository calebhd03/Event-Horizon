using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMetrics : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Slider healthBar; // Reference to the UI Slider for the health bar

    public delegate void HealthChangeAction(float currentHealth, float maxHealth);
    public event HealthChangeAction OnHealthChanged;

    public bool isHealthBarActive = true; // Public toggle for the health bar

    private void Start()
    {
        currentHealth = maxHealth;
        InitializeHealthBar(); // Initialize the health bar
    }

    private void Update()
    {
        // Check for changes in currentHealth and update the health bar accordingly
        if (currentHealth != (healthBar != null ? healthBar.value * maxHealth : 0f))
        {
            UpdateHealthBar();
        }
    }

    public void ModifyHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void InitializeHealthBar()
    {
        // Ensure the health bar exists
        if (healthBar != null)
        {
            // Set the initial state based on the public toggle
            healthBar.gameObject.SetActive(isHealthBarActive);
        }
    }

    public void ToggleHealthBar(bool active)
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(active);
            isHealthBarActive = active; // Update the public toggle when toggling the health bar
        }
    }

    private void UpdateHealthBar()
    {
        // Ensure the health bar exists and is active
        if (healthBar != null && isHealthBarActive)
        {
            // Calculate the normalized value for the slider
            float normalizedHealth = currentHealth / maxHealth;
            healthBar.value = normalizedHealth;
        }
    }
}