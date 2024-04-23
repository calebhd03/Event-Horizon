using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    public bool B1 = true;
    public bool B2 = false;
    public PlayerData playerData;
    public AudioSource audioSource;

    private bool soundPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !soundPlayed)
        {
            // Play sound
            if (audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }

            // Set PickUp1 or PickUp2 based on bool1 or bool2
            if (B1)
            {
                playerData.PickUp1 = true;
            }
            else if (B2)
            {
                playerData.PickUp2 = true;
            }

            // Set soundPlayed to true to ensure it only plays once
            soundPlayed = true;

            // Destroy the game object after playing the sound
            Destroy(gameObject);
        }
    }
}