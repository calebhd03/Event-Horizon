using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
[SerializeField] private Transform vfxHit;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        // Set collision detection mode to Continuous Dynamic
        bulletRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Start()
    {
        float speed = 30f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthMetrics>() != null)
        {
            HealthMetrics healthMetrics = other.GetComponent<HealthMetrics>();

            if (healthMetrics != null)
            {
                Instantiate(vfxHit, transform.position, Quaternion.identity);
                healthMetrics.ModifyHealth(-20f); // Apply 20 damage to the object
            }
            // Handle the hit target logic here, if needed.
        }
        else
        {
            Instantiate(vfxHit, transform.position, Quaternion.identity);
            // Handle hitting something else logic here, if needed.
        }
        Destroy(gameObject);
    }
}






