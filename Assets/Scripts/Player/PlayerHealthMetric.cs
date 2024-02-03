using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMetric : MonoBehaviour
{
    public PlayerData playerData;

    public RectTransform healthBar; // Reference to the UI RectTransform for the health bar

    public delegate void HealthChangeAction(float currentHealth, float maxHealth);
    public event HealthChangeAction OnHealthChanged;

    public bool isHealthBarActive = true; // Public toggle for the health bar

    private SaveSystemTest saveSystemTest;
    public AudioClip healthIncreaseSound;
    public AudioClip healthDecreaseSound;
    AudioSource audioSource;
    public GameObject healthMeter;

    private void Start()
    {
        InitializeHealthBar(); // Initialize the health bar
        saveSystemTest = FindObjectOfType<SaveSystemTest>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check for changes in currentHealth and update the health bar accordingly
        if (playerData.currentHealth != (healthBar != null ? healthBar.sizeDelta.y * playerData.maxHealth : 0f))
        {
            UpdateHealthBar();
        }
        if (playerData.currentHealth > playerData.maxHealth * (2f/3f))
        {
            healthMeter.GetComponent<Image>().color = Color.white;
        }
        if (playerData.currentHealth < playerData.maxHealth * (2f/3f) && playerData.currentHealth > playerData.maxHealth * (1f/3f))
        {
            healthMeter.GetComponent<Image>().color = Color.yellow;
        }
        if (playerData.currentHealth < playerData.maxHealth * (1f/3f))
        {
            healthMeter.GetComponent<Image>().color = Color.red;
        }
    }

    public void ModifyHealth(float amount)
    {
        float previousHealth = playerData.currentHealth;
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth + amount, 0f, playerData.maxHealth);
        OnHealthChanged?.Invoke(playerData.currentHealth, playerData.maxHealth);

        if (playerData.currentHealth <= 0f)
        {
            Debug.Log("Player Health 0");
        }

        if (playerData.currentHealth < previousHealth)
        {
            audioSource.PlayOneShot(healthIncreaseSound);
        }
        else if(playerData.currentHealth > previousHealth)
        {
            audioSource.PlayOneShot(healthDecreaseSound);
        }
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

    public void UpdateHealthBar()
    {
        // Ensure the health bar exists and is active
        if (healthBar != null && isHealthBarActive)
        {
            // Calculate the normalized value for the bar's size
            float normalizedHealth = playerData.currentHealth / playerData.maxHealth;
            healthBar.sizeDelta = new Vector2(healthBar.sizeDelta.x, normalizedHealth * 100f);
        }
    }
}