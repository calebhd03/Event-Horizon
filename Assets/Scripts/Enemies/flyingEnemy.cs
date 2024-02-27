using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class flyingEnemy : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    private Rigidbody rb;
    [SerializeField] EnemyHealthBar healthBar;
    public LayerMask playerZone;

    //check to find player
    private bool iSeeYou;
    public float seeDistance;

    [Header("Patrol")]
    public Transform[] movePoints;
    private int destinationPoints = 0;

    [Header("Attack")]
    public float attackRange;
    private bool withInAttackRange;
    public float moveDistance = 3f;
    private bool attackAgainCoolDown;
    public float attackAgainTimer;

    [Header("Projectiles")]
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float maxMag = 20f;
    private float currentMag;
    public float fireRate = 1.0f;
    private float nextFire;
    public float projectileSpeed = 5;

    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    [Header("Audio")]
    public AudioClip deathAudio;
    AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthMetrics.currentHealth = healthMetrics.maxHealth;
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);
        currentMag = maxMag;
    }

    // Update is called once per frame
    void Update()
    {
        updateHealth();
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        withInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerZone);

        if (iSeeYou == false && withInAttackRange == false)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                Patrol();
            }
        }

        if (iSeeYou == true && withInAttackRange == false)
        {
            transform.LookAt(player);
            chasePlayer();
        }

        if (iSeeYou == true && withInAttackRange == true)
        {
            attackPlayer();
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
    public void Patrol()
    {
        //resets movement
        if (movePoints.Length == 0)
            return;


        agent.destination = movePoints[destinationPoints].position;

        destinationPoints = (destinationPoints + 1) % movePoints.Length;
    }

    private void chasePlayer() //chase player once found
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    public void attackPlayer()
    {
        if (attackAgainCoolDown == false && Time.time >= nextFire)
        {
            //agent.SetDestination(transform.position);

            //fire Rate
            nextFire = Time.time + 1 / fireRate;

            //attack code for range attack
            Rigidbody newBullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();

            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.Normalize();
            newBullet.velocity = directionToPlayer * projectileSpeed;

            animator.SetBool("RangeAttack", true);
            AttackMoving();

            currentMag--;

            if (currentMag <= 0)
            {
                animator.SetBool("RangeAttack", false);

                attackAgainCoolDown = true;

                //reload timer
                Invoke(nameof(ResetProjectiles), attackAgainTimer);
            }

            //destroy bullet properly for now
            Destroy(newBullet.gameObject, 5f);
        }
    }

    private void ResetProjectiles()
    {
        attackAgainCoolDown = false;
        currentMag = maxMag;
        Debug.Log("Max Bullets");
    }

    private void AttackMoving()
    {
        Vector3 rightDestination = agent.transform.position + transform.right * moveDistance;
        Vector3 leftDestination = agent.transform.position - transform.right * moveDistance;

        if (Random.value > 0.5f)
        {
            agent.SetDestination(leftDestination);
        }
        else
        {
            agent.SetDestination(rightDestination);
        }
    }

    public void updateHealth()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
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

    public void SetISeeYou()
    {
        iSeeYou = true;
        transform.LookAt(player);
        chasePlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
}
