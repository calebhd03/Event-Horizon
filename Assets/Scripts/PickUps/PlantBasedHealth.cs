using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBasedHealth : MonoBehaviour
{
    public float pickUpHealthAmount = 10f;
    public float radius = 5f;
    public bool used = false;
    public GameObject cloud;

    void Start()
    {
        //cloud.SetActive(false);
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
                    gameObject.SetActive(false);
                    //cloud.SetActive(true);
                }
            }
        }
    }    
}
