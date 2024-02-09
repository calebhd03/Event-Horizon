using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class crystalEnemy : MonoBehaviour
{
    [SerializeField] EnemyHealthBar healthBar;
    public Transform player;
    public NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;
    public LayerMask obstacleZone;
    public LayerMask playerZone;

    //enemy view in coned shaped
    public float viewRadius;
    public float viewAngle;

    //check to find player
    private bool iSeeYou;

    [Header("Attack")]
    public float attackRange;
    private bool withInAttackRange;

    [Header("Patrol")]
    public Transform[] movePoints;
    private int destinationPoints = 0;

    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    [Header("Audio")]
    public AudioClip deathAudio;
    [Range(0, 10)] public float deathAudioVolume;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthMetrics.currentHealth = healthMetrics.maxHealth;
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        updateHealth();
        Vector3 playerTarget = (player.position - transform.position).normalized;

        if (Vector3.Angle(playerTarget, transform.forward) < viewAngle / 2)
        {
            float distanceTarget = Vector3.Distance(transform.position, player.position);

            if (distanceTarget <= viewRadius && !Physics.Raycast(transform.position, playerTarget, distanceTarget, obstacleZone))
            {
                iSeeYou = true;
                //hearDistance = 0;
                transform.LookAt(player);
                Debug.DrawRay(transform.position, playerTarget * viewRadius * viewAngle, Color.blue); //debug raycast line to show if enemy can see the player
            }

            else
            {
                iSeeYou = false;
            }
        }
        else
        {
            iSeeYou = false;
        }

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
        //Debug field of view of enemy, shows raycast
        DrawFieldOfVision();

    }
    public void Patrol()
    {
        //resets movement
        if (movePoints.Length == 0)
            return;

        agent.destination = movePoints[destinationPoints].position;

        destinationPoints = (destinationPoints + 1) % movePoints.Length;
        //Debug.Log("moving to " + agent.destination);
    }
    private void chasePlayer() //chase player once found
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    private void attackPlayer()
    {
       //attack
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
        AudioSource.PlayClipAtPoint(deathAudio, transform.position, deathAudioVolume);

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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    //Visual representation for debugging the cone of vision of the enemy. Shows the ray cast for debugging
    private void DrawFieldOfVision()
    {
        float halfAngle = viewAngle / 2f;
        Vector3 startPoint = transform.position;
        Vector3 endPointLeft = Quaternion.Euler(0f, -halfAngle, 0f) * transform.forward * viewRadius;
        Vector3 endPointRight = Quaternion.Euler(0f, halfAngle, 0f) * transform.forward * viewRadius;

        Debug.DrawRay(startPoint, endPointLeft, Color.green);
        Debug.DrawRay(startPoint, endPointRight, Color.green);

        Debug.DrawRay(startPoint + endPointLeft, endPointRight - endPointLeft, Color.green);
    }
}
