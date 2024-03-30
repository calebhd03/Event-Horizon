using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{    
        [SerializeField] private Transform vfxHit;

    private Rigidbody bulletRigidbody;
    private Vector3 lastPosition;
    public float speed = 30f;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        // Set collision detection mode to Continuous Dynamic
        bulletRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Perform a Linecast from the current position to the last position of the bullet
        RaycastHit hitInfo;
        if (Physics.Linecast(lastPosition, transform.position, out hitInfo))
        {
            // Check if the collider is on any of the specified layers
            if (IsLayerIgnored(hitInfo.collider.gameObject.layer))
            {
                // Do nothing if the collider is on the specified layers
                return;
            }

            // Debug what the bullet hit
            Debug.Log("Bullet hit: " + hitInfo.collider.gameObject.name);

            // Handle collision with bubbles
            if (hitInfo.collider.CompareTag("Bubbles"))
            {
               // Instantiate(vfxHit, hitInfo.point, Quaternion.identity);
                Debug.Log("In Bubbles");
                return;
            }
            else
            {
                // Handle collision with other objects
                HealthMetrics healthMetrics = hitInfo.collider.GetComponent<HealthMetrics>();
                if (healthMetrics != null)
                {
                    Instantiate(vfxHit, hitInfo.point, Quaternion.identity);
                    // Damage done on enemy hit boxes with tag bullets
                    healthMetrics.ModifyHealth(-20f); // Apply 20 damage to the object
                }
                else
                {
                    Instantiate(vfxHit, hitInfo.point, Quaternion.identity);
                    // Handle hitting something else logic here, if needed.
                }

                Destroy(gameObject);
                return;
            }
        }

        // Update the last position after the Linecast
        lastPosition = transform.position;
    }

    private bool IsLayerIgnored(int layer)
    {
        // List of layers to ignore
        int[] ignoredLayers = new int[]
        {
            LayerMask.NameToLayer("Bullets"),
            LayerMask.NameToLayer("CheckPoints"),
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("GunLayer"),
            LayerMask.NameToLayer("WallBullet"),
            LayerMask.NameToLayer("EnemyCollider"),
            LayerMask.NameToLayer("EnemyLayer"),
            LayerMask.NameToLayer("Dialog")
        };

        // Check if the layer is in the ignored layers
        foreach (int ignoredLayer in ignoredLayers)
        {
            if (layer == ignoredLayer)
            {
                return true;
            }
        }

        return false;
    }
}