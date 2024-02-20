using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
[SerializeField] private Transform vfxHit;

    private Rigidbody laserRigidbody;
    private Vector3 lastPosition;
    public float speed = 30f;
    int numberOfEnemiesHit = 0;

    private void Awake()
    {
        laserRigidbody = GetComponent<Rigidbody>();
        // Set collision detection mode to Continuous Dynamic
        laserRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Start()
    {
        laserRigidbody.velocity = transform.forward * speed;
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        int layerMask = ~(LayerMask.GetMask("Bullets", "CheckPoints", "Player", "GunLayer","WallBullet","EnemyColider","EnemyLayer"));
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
            layerMask == LayerMask.NameToLayer("GunLayer")||
            layerMask == LayerMask.NameToLayer("WallBullet")||
            layerMask == LayerMask.NameToLayer("EnemyColider")||
            layerMask == LayerMask.NameToLayer("EnemyLayer"))

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
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemiesHit += 1;
        }
        if (numberOfEnemiesHit == 4)
        {
        Destroy(gameObject);
        }
        else
        {
            Invoke("DestroyBullet", 5);
        }
    }
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
