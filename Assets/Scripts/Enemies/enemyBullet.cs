using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float bulletDamage = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            HealthMetrics healthMetrics = other.GetComponent<HealthMetrics>();

            if(healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-bulletDamage);
            }
            Destroy(gameObject);
        }
    }
}
