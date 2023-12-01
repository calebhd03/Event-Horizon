using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickUp : MonoBehaviour
{
    public float pickUpHealthAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthMetric playerHealth = other.GetComponent<PlayerHealthMetric>();
            if (playerHealth != null)
            {
                /*play sounds effects or show visuals effect would be added
                 * in here*/
                playerHealth.ModifyHealth(pickUpHealthAmount);
                Destroy(gameObject);
            }
        }
    }
}
