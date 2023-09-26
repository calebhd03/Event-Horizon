using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthMetrics>() != null)
        {
            HealthMetrics healthMetrics = other.GetComponent<HealthMetrics>();

            if (healthMetrics != null)
            {
                healthMetrics.ModifyHealth(-20f); // Apply 20 damage to the object
            }
            // Handle the hit target logic here, if needed.
        }
        else
        {
            // Handle hitting something else logic here, if needed.
        }

        Destroy(gameObject);
    }
}






