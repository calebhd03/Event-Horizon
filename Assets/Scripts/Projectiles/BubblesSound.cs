using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesSound : MonoBehaviour
{
     public AudioSource audioSource;
    public ParticleSystem particleSystem; // Public field to reference the particle system

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found. Make sure it's attached to the GameObject.");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        // Check if the collision involves the particle system
        if (other == particleSystem.gameObject)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                Debug.Log("Pop");
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource or AudioClip is missing. Make sure they are properly set up.");
            }
        }
    }
}