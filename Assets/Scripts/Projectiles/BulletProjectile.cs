using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
   [SerializeField] private Transform vfxHit;

    private Rigidbody bulletRigidbody;
    private Vector3 lastPosition;

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
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer"));
        if (Physics.Linecast(transform.position, lastPosition, out RaycastHit hitInfo, layerMask))
        {
            transform.position = lastPosition;
            OnTriggerEnter(hitInfo.collider);
            Debug.Log("Raycast triggered");
        }
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        int layerMask = other.gameObject.layer;
        // Check if the collider is on any of the specified layers
        if (layerMask == LayerMask.NameToLayer("Bullets") ||
            layerMask == LayerMask.NameToLayer("CheckPoints") ||
            layerMask == LayerMask.NameToLayer("Player") ||
            layerMask == LayerMask.NameToLayer("GunLayer"))
        {
            // Do nothing if the collider is on the specified layers
            return;
        }

        Debug.LogWarning("hit " + other);
        if (other.GetComponent<HealthMetrics>() != null)
        {
            HealthMetrics healthMetrics = other.GetComponent<HealthMetrics>();

            if (healthMetrics != null)
            {
                Instantiate(vfxHit, transform.position, Quaternion.identity);
                // damage done on enemy hit boxes with tag bullets
                // healthMetrics.ModifyHealth(-20f); // Apply 20 damage to the object
            }
            // Handle the hit target logic here, if needed.
        }
        else
        {
            Instantiate(vfxHit, lastPosition, Quaternion.identity);
            // Handle hitting something else logic here, if needed.
        }
        Destroy(gameObject);
    }
}






