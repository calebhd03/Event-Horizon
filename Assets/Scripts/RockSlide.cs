using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSlide : MonoBehaviour
{ 
   public ParticleSystem rockEffect;
    public ParticleSystem dustEffect;
    public GameObject block;
    public GameObject goal;
    public GameObject trigger;

   
    public GameObject objectiveChange;

    private bool isTriggerActive = true;
    private bool isGoalActive = true;
    private bool hasBeenTriggered = false;

    void Update()
    {
        if (!isTriggerActive && !rockEffect.isPlaying)
        {
            block.SetActive(true);
        }

        if (!isGoalActive)
        {
            StopEffectsAndBlock();
        }

       
        if (!goal.activeSelf)
        {
            StopEffectsAndBlock();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
            isTriggerActive = false;
            rockEffect.Play();
            dustEffect.Play();

         
            if (objectiveChange != null)
            {
                objectiveChange.SetActive(true);
            }
        }
    }

    void StopEffectsAndBlock()
    {
        rockEffect.Stop();
        dustEffect.Stop();
        block.SetActive(false);
    }
}