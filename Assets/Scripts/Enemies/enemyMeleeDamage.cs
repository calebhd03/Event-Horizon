using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMeleeDamage : MonoBehaviour
{
    public float meleeDamage = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealthMetric healthMetric = other.GetComponent<PlayerHealthMetric>();

            if(healthMetric != null)
            {
                healthMetric.ModifyHealth(-meleeDamage);
            }
        }
    }

}
