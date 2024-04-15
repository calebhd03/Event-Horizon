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
    
    // Attack configuration
    public float attack1Cooldown = 5f;
    public float attack2Cooldown = 7f;
    public float attack3Cooldown = 10f;

    private float attack1Timer;
    private float attack2Timer;
    private float attack3Timer;

    public Transform player; // Reference to the player's transform
    public GameObject bubbleProjectile; // Bubble projectile prefab
    public float bubbleRange = 10.0f; // Range within which to perform the bubble attack
    public float bubbleProjectileSpeed = 15.0f; // Speed of the bubble projectile
    public int bubbleAttackDuration = 12; // Duration of the bubble attack phase
    public float bubbleShotInterval = 3.0f; // Interval between bubble shots
    public float bubbleAttackCooldown = 8.0f; // Cooldown after the bubble attack


    public Vector3 splashAttackPosition; // Position to move to for Splash Attack
    public GameObject reticlePrefab; // Prefab for the reticle object
    public GameObject splashSeedPrefab; // Prefab for the Splash Seed projectile
    public float splashAttackCooldown = 9.0f; // Cooldown duration after Splash Attack
    private GameObject reticleInstance;

    public GameObject bubbleWallPrefab; // Prefab for the Bubble Wall particle effect
    public GameObject doublePrefab; // Prefab for the doubles of the enemy
    public float schoolAttackCooldown = 7.0f; // Cooldown for the School Phase attack
    private List<GameObject> doubles = new List<GameObject>();

    void Start()
    {
        if (waypoints.Count > 0)
            targetWaypoint = waypoints[currentWaypointIndex];
    }

    void Update()
    {
        if (!fight)
        {
            MoveBetweenWaypoints();
        }
        else
        {
            if (attack1Timer > 0) attack1Timer -= Time.deltaTime;
            if (attack2Timer > 0) attack2Timer -= Time.deltaTime;
            if (attack3Timer > 0) attack3Timer -= Time.deltaTime;

            // Execute attacks if timers are zero and no other attack is active
            if (attack1Timer <= 0 && attack2Timer > 0 && attack3Timer > 0)
            {
                StartCoroutine(PerformAttack1());
            }
            else if (attack2Timer <= 0 && attack1Timer > 0 && attack3Timer > 0)
            {
                StartCoroutine(PerformAttack2());
            }
            else if (attack3Timer <= 0 && attack1Timer > 0 && attack2Timer > 0)
            {
                StartCoroutine(PerformAttack3());
            }
            else
            {
                MoveToBottomOfBounds();
            }
        }
    }

    IEnumerator PerformAttack1()
    {
        Debug.Log("Performing Attack 1");
      
    
        float attackTime = 0f;

        // Move to attack range
        while (Vector3.Distance(transform.position, player.position) > bubbleRange)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            Vector3 newPos = transform.position + dirToPlayer * speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
            yield return null; // Wait for next frame
        }

        // Start attacking
        while (attackTime < bubbleAttackDuration)
        {
            if (Vector3.Distance(transform.position, player.position) <= bubbleRange)
            {
                // Spawn and shoot a bubble projectile
                GameObject projectile = Instantiate(bubbleProjectile, transform.position, Quaternion.identity);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                Vector3 shootDir = (player.position - transform.position).normalized;
                rb.velocity = shootDir * bubbleProjectileSpeed;

                attackTime += bubbleShotInterval;
                yield return new WaitForSeconds(bubbleShotInterval); // Wait for interval before next shot
            }
            else
            {
                // Move back into range if player moved away
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Cooldown phase
        Debug.Log("Bubble attack cooldown started.");
        attack1Timer = bubbleAttackCooldown;

    }

    IEnumerator PerformAttack2()
    {
        Debug.Log("Performing Attack 2");
           // Move to the specific coordinates
        while (Vector3.Distance(transform.position, splashAttackPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, splashAttackPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Stop movement and orient towards the player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Create the reticle and follow the player
        if (reticlePrefab)
        {
            reticleInstance = Instantiate(reticlePrefab, new Vector3(player.position.x, 0, player.position.z), Quaternion.identity);
        }

        float trackingTime = 0f;
        while (trackingTime < 5f)
        {
            if (reticleInstance)
            {
                Vector3 playerFloorPosition = new Vector3(player.position.x, 0, player.position.z);
                reticleInstance.transform.position = Vector3.Lerp(reticleInstance.transform.position, playerFloorPosition, 0.1f); // 1-second delay in following
            }
            trackingTime += Time.deltaTime;
            yield return null;
        }

        // Lock the reticle and spawn the Splash Seed
        if (splashSeedPrefab && reticleInstance)
        {
            Vector3 seedSpawnPosition = reticleInstance.transform.position + Vector3.up * 60;
            Instantiate(splashSeedPrefab, seedSpawnPosition, Quaternion.identity);
        }

        // Wait 3 seconds
        yield return new WaitForSeconds(3);

        // Clean up the reticle
        if (reticleInstance)
        {
            Destroy(reticleInstance);
        }

        // Start cooldown
        attack2Timer = splashAttackCooldown;
    
    }

    IEnumerator PerformAttack3()
    {
        Debug.Log("Performing Attack 3");
            // Instantiate the Bubble Wall effect
        GameObject bubbleWall = Instantiate(bubbleWallPrefab, transform.position, Quaternion.identity);
        
        // Instantiate doubles
        int numberOfDoubles = Random.Range(2, 5); // Generates 2 to 4 doubles
        Vector3 basePosition = transform.position - new Vector3(1f * numberOfDoubles / 2, 0, 0); // Start position for the first double
        for (int i = 0; i < numberOfDoubles; i++)
        {
            Vector3 doublePosition = basePosition + new Vector3(1f * i, 0, 0); // Position each double next to each other
            GameObject newDouble = Instantiate(doublePrefab, doublePosition, Quaternion.identity);
            doubles.Add(newDouble);
        }

        // Wait until all doubles are destroyed or 15 seconds pass
        float elapsedTime = 0;
        while (elapsedTime < 15f && doubles.Exists(d => d != null))
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Clean up
        foreach (GameObject d in doubles)
        {
            if (d != null)
            {
                Destroy(d);
            }
        }
        doubles.Clear();

        if (bubbleWall != null)
        {
            Destroy(bubbleWall);
            // Instantiate the Bubble Wall effect again
            Instantiate(bubbleWallPrefab, transform.position, Quaternion.identity);
        }

        // Start cooldown
        attack3Timer = schoolAttackCooldown;

    }

    void MoveToBottomOfBounds()
    {
        Vector3 bottomPosition = new Vector3(transform.position.x, minBounds.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, bottomPosition, speed * Time.deltaTime);
    }

    public void SetFightTrue()
    {
        fight = true;
    }

    public void SetFightFalse()
    {
        fight = false;
    }

    void MoveBetweenWaypoints()
    {
        if (targetWaypoint == null)
            return;

        Vector3 targetPosition = targetWaypoint.position;
        // Ensure that the target position is at the same Y level as the enemy to avoid tilting up or down.
        targetPosition.y = transform.position.y;

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate only on the Y axis
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
        if (flatDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Check for proximity to the waypoint considering only the X and Z coordinates
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
    }

    void MoveInBounds()
    {
        Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;
        newPosition = ClampPosition(newPosition);
        transform.position = newPosition;
    }

    Vector3 ClampPosition(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        position.z = Mathf.Clamp(position.z, minBounds.z, maxBounds.z);
        return position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((minBounds + maxBounds) / 2, maxBounds - minBounds);
    }
}