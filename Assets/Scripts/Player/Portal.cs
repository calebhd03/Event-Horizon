using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Portal : MonoBehaviour
{
    public string nextSceneName; // Name of the scene to load
    public SceneTransitionController sceneTransition;
    public ParticleSystem portalParticle; // Reference to the Particle System

    private void Awake()
    {
        // Disable the Portal object at the start
        gameObject.SetActive(false);

        // Ensure the particle system is not playing on awake
        if (portalParticle != null)
        {
            portalParticle.Stop();
        }
    }

    private void OnEnable()
    {
        AndDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !sceneTransition.IsFading())
        {
            // Load the game data (assuming SaveSystemTest is attached to the player)
            SaveSystemTest saveSystem = FindObjectOfType<SaveSystemTest>();
            if (saveSystem != null)
            {
                saveSystem.SaveGame();
                saveSystem.LoadGame();
            }

            // Play the particle effect
            if (portalParticle != null)
            {
                portalParticle.Play();
            }

            // Trigger the fade-out effect and scene transition
            sceneTransition.StartCoroutine("FadeIn", nextSceneName);

            Destroy(other.gameObject);
        }
    }

    public void AndDestroy()
    {
        // Find and destroy all objects tagged as "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}