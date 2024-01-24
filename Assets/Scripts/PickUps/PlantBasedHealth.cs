using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBasedHealth : MonoBehaviour
{
    public float pickUpHealthAmount = 10f;
    public float radius = 5f;
    public bool used = false;
    ParticleSystem cloud;
    Renderer mesh;


    void Start()
    {
        cloud = GetComponent<ParticleSystem>();
        mesh = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used == false)
        {
            if(other.CompareTag("Player"))
            {
                PlayerHealthMetric playerHealth = other.GetComponent<PlayerHealthMetric>();
                if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
                {
                    used = true;
                    playerHealth.ModifyHealth(pickUpHealthAmount);
                    mesh.enabled = false;
                    cloud.Play();
                }
            }
        }
    }    
}
