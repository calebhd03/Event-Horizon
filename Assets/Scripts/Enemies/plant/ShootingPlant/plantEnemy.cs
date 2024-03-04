using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantEnemy : MonoBehaviour
{
    //Seeing playing
    public LayerMask playerZone;
    private bool iSeeYou;
    public float seeDistance;

    public Transform player;
    //health
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] private HealthMetrics healthMetrics;

    [Header("Attack")]
    public GameObject plantProjectilePrefab;
    public Transform projectileSpawn;
    public float windUp = 1f;
    public float coolDown = 1f;
    public float projectileSpeed = 5f;
    private bool canShoot = true;


    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip projectileSpawnSound;
    public AudioClip deathAudio;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthMetrics.currentHealth = healthMetrics.maxHealth;
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        updateHealth();
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        if(iSeeYou == true)
        {
            if (healthMetrics.currentHealth > 0 && canShoot == true)
            {
                StartCoroutine(ShootProjectile());
            }

            else if(healthMetrics.currentHealth <= 0)
            {
                StopCoroutine(ShootProjectile());
            }

            transform.LookAt(player);
            //projectileSpawn.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }   
    }

    private IEnumerator ShootProjectile()
    {
        canShoot = false;

        // Windup time
        yield return new WaitForSeconds(windUp);

        Rigidbody newProjectile = Instantiate(plantProjectilePrefab, projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();

        Vector3 directionToPlayer = (player.position + Vector3.up * 0.5f) - projectileSpawn.position;
        Vector3 velocity = CalculateProjectileArc(directionToPlayer, projectileSpeed);
        newProjectile.velocity = velocity;

        Destroy(newProjectile.gameObject, 5f);

        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }

    private Vector3 CalculateProjectileArc(Vector3 targetDirection, float speed)
    {
        float timeOfFlight = targetDirection.magnitude / speed;

        float initialVelocityX = targetDirection.x / timeOfFlight;
        float initialVelocityY = (targetDirection.y + 0.5f * Mathf.Abs(Physics.gravity.y) * timeOfFlight * timeOfFlight) / timeOfFlight;

        Vector3 velocity = new Vector3(initialVelocityX, initialVelocityY, targetDirection.z / timeOfFlight);

        return velocity;
    }

    public void updateHealth()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            Die();
            Debug.Log("Zero Health");
        }
    }

    public void Die()
    {
        StartCoroutine(WaitAndDropStuff(3f));
    }

    private IEnumerator WaitAndDropStuff(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource.PlayOneShot(deathAudio);

        // Call DropStuff after waiting for 3 seconds
        DropStuff();
    }

    private void DropStuff()
    {
        if (Random.value < pickupDropChance)
        {
            Instantiate(shotGunPickupPrefab, transform.position, Quaternion.identity);
            Instantiate(blasterPickupPrefab, transform.position, Quaternion.identity);
            Instantiate(bHPickupPrefab, transform.position, Quaternion.identity);
        }

        if (Random.value < pickupDropChance / 2)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }

        Destroy(transform.parent.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
}
