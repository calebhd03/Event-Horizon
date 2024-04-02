using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlide : MonoBehaviour
{
    public ParticleSystem rockEffect;
    public ParticleSystem dustEffect;
    public GameObject block;
    public GameObject goal;

    private bool isTriggerActive = true;
    private bool isGoalDestroyed = false;
    private bool hasBeenTriggered = false; // New variable to track if the trigger has been activated

    void Update()
    {
        if (!isTriggerActive && !rockEffect.isPlaying)
        {
            block.SetActive(true);
        }

        if (goal == null)
        {
            StopEffectsAndBlock();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player")) // Check if the trigger hasn't been activated yet
        {
            hasBeenTriggered = true; // Mark the trigger as activated
            isTriggerActive = false;
            rockEffect.Play();
            dustEffect.Play();
        }
    }

    void StopEffectsAndBlock()
    {
        rockEffect.Stop();
        dustEffect.Stop();
        block.SetActive(false);
    }
}