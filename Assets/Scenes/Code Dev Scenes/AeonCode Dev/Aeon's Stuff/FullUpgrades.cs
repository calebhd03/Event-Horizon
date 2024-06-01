using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullUpgrades : MonoBehaviour
{

    public PlayerData playerData;
    public AudioSource audioSource;
    private bool soundPlayed = false;

    [Header("Rotation")]
    public float rotationSpeed = 100f; // Adjustable rotation speed

    private void Start()
    {
        // Start the rotation
        StartCoroutine(Rotate());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !soundPlayed)
        {
            playerData.FullUpgrades();
            
            // Play sound
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }

            // Set soundPlayed to true to ensure it only plays once
            soundPlayed = true;

            // Destroy the game object after playing the sound
            Destroy(gameObject);
        }
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            // Rotate the object around its Y-axis at the specified speed
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}