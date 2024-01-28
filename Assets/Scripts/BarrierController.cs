using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
        public GameObject particleSystemPrefab; // Particle system prefab
    public GameObject cutscene; // Add this variable to store the cutscene prefab
    public bool shouldPlayCutscene = false; // Add this variable

    public void DestroyBarrier()
    {
        StartCoroutine(DestroyAndInstantiate());
    }

    IEnumerator DestroyAndInstantiate()
    {
        // Instantiate Particle System at the top of the game object
        Vector3 particleSystemPosition = transform.position + Vector3.up * (particleSystemPrefab.GetComponent<ParticleSystem>().main.startLifetime.constant + 5.0f);
        GameObject particleSystemInstance = Instantiate(particleSystemPrefab, particleSystemPosition, Quaternion.identity);

        // Enable the Particle System
        ParticleSystem particleSystem = particleSystemInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        // Enable MeshRenderer and apply forces when needed (for visualization purposes)
        MeshRenderer meshRenderer = particleSystemInstance.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }

        // Destroy the current Barrier GameObject
        Destroy(gameObject);

        // Wait for the particle system to finish (replace this with the actual duration of your particle system)
        yield return new WaitForSeconds(particleSystem.main.duration);

        // Instantiate cutscene if shouldPlayCutscene is true
        if (shouldPlayCutscene && cutscene != null)
        {
            Instantiate(cutscene, transform.position, Quaternion.identity);
        }

        // Destroy the current Barrier GameObject
        // Destroy(gameObject);
    }

    public void MemoryLog()
    {
        LogSystem logSystem = FindObjectOfType<LogSystem>();
        logSystem.UpdateMemoryLog();
    }
}