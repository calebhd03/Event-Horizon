using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleCollisionHandler : MonoBehaviour
{
   private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        CheckParticleTriggers();
    }

    private void CheckParticleTriggers()
    {
        if (particles == null)
        {
            Debug.LogWarning("Particle system not assigned.");
            return;
        }

        // Get the collisions from the particle system
        List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();
        int numCollisions = particles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);

        // Loop through each trigger collision
        for (int i = 0; i < numCollisions; i++)
        {
            ParticleSystem.Particle particle = particleList[i];
            // Get the position of the particle
            Vector3 particlePosition = particle.position;

            // Check for triggers at the particle's position
            Collider[] colliders = Physics.OverlapSphere(particlePosition, 2f);
            foreach (Collider collider in colliders)
            {
                // Check if the collider is a trigger
                if (collider.isTrigger)
                {
                    // Handle trigger collision
                    Debug.Log("Particle triggered " + collider.gameObject.name);
                    // For example, you can call a function to destroy the particle
                    particles.Emit(i); // Emit the particle again to reset its lifetime
                    break;
                }
            }
        }
    }
}