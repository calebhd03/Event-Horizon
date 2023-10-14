using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weakPoint : MonoBehaviour
{
    public float weakPointDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HealthMetrics healthMetrics = GetComponent<HealthMetrics>();

            if (healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-weakPointDamage);
                Debug.Log("A  WeakPoint");
            }


            //Destroy(gameObject);
        }
    }
}
