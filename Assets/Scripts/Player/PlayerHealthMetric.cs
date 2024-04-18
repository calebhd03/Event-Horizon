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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (healthBar != null)
        {
            // Use material directly to change instance-specific material properties
            healthBarMaterial = healthBar.GetComponent<Renderer>().material;
        }
        InitializeHealthBar();
        playerData.InitializeArrays();
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void ModifyHealth(float amount)
    {
        float previousHealth = playerData.currentHealth;
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth + amount, 0, playerData.maxHealth);

        UpdateHealthBar(); // Update the bar whenever health is modified

        if (playerData.currentHealth <= 0)
        {
            Debug.Log("Player Health 0");
        }

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
        if (healthBarMaterial != null)
        {
            float healthPercent = playerData.currentHealth / playerData.maxHealth;
            Color color = CalculateHealthColor(healthPercent);
            healthBarMaterial.color = color;
            healthBarMaterial.SetColor("_EmissionColor", color);
        }
    }

    private Color CalculateHealthColor(float healthPercent)
    {
        if (healthPercent > 0.8f)
            return Color.green; // 100%-80%
        else if (healthPercent > 0.58f)
            return Color.green; // Greenish yellow (Lime) - Adjust RGB as needed
        else if (healthPercent > 0.35f)
            return Color.yellow; // 57%-35%
        else if (healthPercent > 0.15f)
            return new Color(1, 0.5f, 0); // Orange
        else
            return Color.red; // Deep red
    }
}