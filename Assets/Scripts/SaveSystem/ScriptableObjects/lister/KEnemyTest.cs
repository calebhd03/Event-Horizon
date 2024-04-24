using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KEnemyTest : MonoBehaviour
{   
    public float speed = 5.0f;
    public float rotationSpeed = 2.0f;
    public List<Transform> waypoints;
    public Vector3 minBounds = new Vector3(-20, -20, -20);
    public Vector3 maxBounds = new Vector3(20, 20, 20);
    private int currentWaypointIndex = 0;

    private Transform targetWaypoint;
    public bool fight = false;

    public Transform player; // Reference to the player's transform
    public GameObject bubbleBulletPrefab; // Bubble bullet prefab
    public float bubbleBulletSpeed = 10.0f; // Speed of the bubble bullet
    public float shootInterval = 1.0f; // Interval between shooting bubble bullets
    public float distanceFromPlayer = 10.0f; // Distance to maintain from the player
    public float yOffsetAbovePlayer = 5.0f; // Editable distance above the player
    public float xOffsetFromPlayer = 3.0f; // Editable horizontal offset from the player
    public Transform bubbleBulletSpawnPoint; // Public spawn point for the bubble bullet
    public GameObject portal; // Portal game object
    public GameObject rewards; // Rewards game object

    private float shootTimer = 0.0f;
    private Vector3 centerPosition; // Center position of the bounds

    private void Start()
    {
        if (waypoints.Count > 0)
            targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the center position of the bounds
        centerPosition = (minBounds + maxBounds) / 2;
    }

    private void Update()
    {
        if (!fight)
        {
            MoveBetweenWaypoints();
        }
        else
        {
            // Switch between moving to the player and moving between waypoints
            float randomValue = Random.value;
            if (randomValue < 0.5f)
            {
                MoveTowardsPlayer();
            }
            else
            {
                MoveBetweenWaypoints();
            }

            ShootAtPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null)
            return;

        // Calculate the target position based on the player's position
        Vector3 targetPosition = player.position;
        targetPosition.y = Mathf.Clamp(targetPosition.y + yOffsetAbovePlayer, minBounds.y, maxBounds.y); // Move to y distance above the player
        targetPosition.x = Mathf.Clamp(targetPosition.x + xOffsetFromPlayer, minBounds.x, maxBounds.x); // Move to x distance away from the player

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate rotation to face the player
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));

        // Apply rotation only around the Y-axis
        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
    }

    private void ShootAtPlayer()
    {
        if (player == null || bubbleBulletPrefab == null)
            return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            if (bubbleBulletSpawnPoint != null)
            {
                // Instantiate bubble bullet at the spawn point
                GameObject bullet = Instantiate(bubbleBulletPrefab, bubbleBulletSpawnPoint.position, Quaternion.identity);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                // Calculate direction towards the player
                Vector3 direction = (player.position - bubbleBulletSpawnPoint.position).normalized;
                rb.velocity = direction * bubbleBulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bubble bullet spawn point is not assigned!");
            }

            // Reset the shoot timer
            shootTimer = shootInterval;
        }
    }

    private void MoveBetweenWaypoints()
    {
        if (targetWaypoint == null)
            return;

        Vector3 targetPosition = targetWaypoint.position;
        targetPosition.y = transform.position.y;

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetWaypoint.position.x, targetWaypoint.position.z)) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == targetWaypoint)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetWaypoint = waypoints[currentWaypointIndex];
        }

        if (other.CompareTag("Bullet"))
        {
            ModifyHealth(-10f, 0); // Decrease health by 10 for a regular bullet
        }
        else if (other.CompareTag("Plasma Bullet"))
        {
            ModifyHealth(-15f, 0); // Decrease health by 15 for a plasma bullet
        }
        else if (other.CompareTag("BHBullet"))
        {
            ModifyHealth(-20f, 1); // Decrease health by 20 for a BHBullet
        }
    }

    private void ModifyHealth(float amount, int weaponType)
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        if (healthMetrics != null)
        {
            healthMetrics.ModifyHealth(amount, weaponType);
            if (healthMetrics.currentHealth <= 0)
            {
                fight = false;
              //  portal.SetActive(true);
               // rewards.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Parent object does not have a HealthMetrics component.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((minBounds + maxBounds) / 2, maxBounds - minBounds);
    }
}