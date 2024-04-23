using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMetric : MonoBehaviour
{
   public PlayerData playerData;
    public GameObject healthBar; // Reference to the 3D GameObject acting as the health bar
    public AudioClip healthIncreaseSound;
    public AudioClip healthDecreaseSound;
    private AudioSource audioSource;
    private Material healthBarMaterial; // Material of the health bar for color changing
    private bool isFlashing = false; // State flag for flashing

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (healthBar != null)
        {
            healthBarMaterial = healthBar.GetComponent<Renderer>().material;
        }
        InitializeHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void ModifyHealth(float amount)
    {
        float previousHealth = playerData.currentHealth;
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth + amount, 0, playerData.maxHealth);

        if (playerData.currentHealth <= 0)
        {
            Debug.Log("Player Health 0");
            if (isFlashing)
            {
                CancelInvoke("ToggleFlashColor");
                isFlashing = false;
            }
        }

        UpdateHealthBar(); // Update the bar whenever health is modified
        PlayHealthChangeSound(previousHealth);
    }

    private void PlayHealthChangeSound(float previousHealth)
    {
        if (playerData.currentHealth < previousHealth)
        {
            audioSource.PlayOneShot(healthDecreaseSound);
        }
        else if (playerData.currentHealth > previousHealth)
        {
            audioSource.PlayOneShot(healthIncreaseSound);
        }
    }

    private void InitializeHealthBar()
    {
        if (healthBarMaterial != null)
        {
            Color color = CalculateHealthColor(playerData.currentHealth / playerData.maxHealth);
            healthBarMaterial.color = color;
            healthBarMaterial.SetColor("_EmissionColor", color);
        }
    }

    public void UpdateHealthBar()
    {
        float healthPercent = playerData.currentHealth / playerData.maxHealth;
        Color color = CalculateHealthColor(healthPercent);

        if (healthBarMaterial != null && !isFlashing)
        {
            healthBarMaterial.color = color;
            healthBarMaterial.SetColor("_EmissionColor", color);
        }

        // Manage flashing state for health 10% or below
        if (healthPercent <= 0.10f && !isFlashing)
        {
            isFlashing = true;
            InvokeRepeating("ToggleFlashColor", 0, 0.5f); // Start flashing
        }
        else if (healthPercent > 0.10f && isFlashing)
        {
            CancelInvoke("ToggleFlashColor");
            isFlashing = false;  // Reset flashing state
            // Reset to the correct color
            healthBarMaterial.color = color;
            healthBarMaterial.SetColor("_EmissionColor", color);
        }
    }

    private Color CalculateHealthColor(float healthPercent)
    {
        if (healthPercent > 0.82f)
            return Color.green; 
        else if (healthPercent > 0.60f)
            return Color.yellow; 
        else if (healthPercent > 0.45f)
            return new Color(1, 0.5f, 0); 
        else
            return Color.red; 
    }

    void ToggleFlashColor()
    {
        // Toggle between red and black
        if (healthBarMaterial.color == Color.red)
        {
            healthBarMaterial.color = Color.black;
            healthBarMaterial.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            healthBarMaterial.color = Color.red;
            healthBarMaterial.SetColor("_EmissionColor", Color.red);
        }
    }
}