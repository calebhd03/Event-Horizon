using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMeleeDamage : MonoBehaviour
{
    public float meleeDamage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            HealthMetrics healthMetrics = other.GetComponent<HealthMetrics>();

            if(healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-meleeDamage);
            }
        }
    }

}
