using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Steamworks;

public class Feesh : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
   // public Animator animator;
    private Rigidbody rb;
    [SerializeField] EnemyHealthBar healthBar;
    public LayerMask playerZone;
    [SerializeField] private HealthMetrics healthMetrics;

    //check to find player
    private bool iSeeYou;
    public float seeDistance;

    public GameObject particleEffectPrefab;
    public GameObject HealthBar;

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
    public GameObject[] Prize;

    [Header("Audio")]
    public AudioClip deathAudio;
    //public AudioClip rangedAudio;
    //public AudioClip attackAudio;
    AudioSource audioSource;

    private bool isDead = false;//assuming it is alive

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(EnemyMusic());
    }

    // Start is called before the first frame update
    void Start()
    {
        healthMetrics = GetComponentInParent<HealthMetrics>();
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
            //audioSource.PlayOneShot(rangedAudio);
            transform.LookAt(player);
            chasePlayer();
        }

        if (iSeeYou == true && withInAttackRange == true)
        {
            attackPlayer();
            //audioSource.PlayOneShot(rangedAudio);
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        if(healthMetrics.currentHealth <= 0)
        {
            iSeeYou = false;
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
        if (healthMetrics.currentHealth > 0 && attackAgainCoolDown == false && Time.time >= nextFire)
        {
            //agent.SetDestination(transform.position);

            //fire Rate
            nextFire = Time.time + 1 / fireRate;

            //attack code for range attack
            Rigidbody newBullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();

            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.Normalize();
            newBullet.velocity = directionToPlayer * projectileSpeed;

        
            AttackMoving();

            currentMag--;

            if (currentMag <= 0)
            {

                attackAgainCoolDown = true;

                //reload timer
                Invoke(nameof(ResetProjectiles), attackAgainTimer);
            }

            
            //destroy bullet properly for now
            Destroy(newBullet.gameObject, 5f);
        }

        else if (healthMetrics.currentHealth <= 0)
        {
            attackAgainCoolDown = true;
            currentMag = 0;
            maxMag = 0;
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
        Debug.Log("MOVEEEEEEE");
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
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            isDead = true;
            Die();
            Debug.Log("Zero Health");
        }
    }

    public void Die()
    {
        agent.isStopped = true;
        iSeeYou = false;
        Destroy(HealthBar);
        StartCoroutine(MoveAndSpin());
    }

    private IEnumerator MoveAndSpin()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 10f;
        float duration = 11f;
        float elapsedTime = 0f;
        float startSpinSpeed = 1f;
        float endSpinSpeed = 20f;
        float totalRotations = 5f; // Number of full rotations

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Move up
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Spin
            float currentSpinSpeed = Mathf.Lerp(startSpinSpeed, endSpinSpeed, t);
            float angle = Mathf.Lerp(0, totalRotations * 360f, t); // Full rotation over time
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Instantiate particle effect
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);

        foreach (GameObject prize in Prize)
        {
            if (prize != null)
            {
                prize.SetActive(true);
            }
        }

        // Destroy the game object
        Destroy(gameObject);
    }





    public void SetISeeYou()
    {
        iSeeYou = true;
        transform.LookAt(player);
        if (iSeeYou && !withInAttackRange)
        {
            chasePlayer();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
    /*IEnumerator EnemyMusic()
    {
        yield return new WaitUntil(() => iSeeYou);
        Background_Music.instance.IncrementSeeingPlayerCount();
        StartCoroutine(LevelMusic());
        yield return null;
    }
    IEnumerator LevelMusic()
    {   
        yield return new WaitUntil (() => !iSeeYou);
        Background_Music.instance.DecrementSeeingPlayerCount();
        StartCoroutine(EnemyMusic());
        yield return null;
    }*/

    public void Dead()
    {
        if(SteamManager.Initialized)
        {
            int currentEnemyKills;
            Steamworks.SteamUserStats.GetStat("STAT_ENEMIES_KILLED", out currentEnemyKills);
            currentEnemyKills++;
            Steamworks.SteamUserStats.SetStat("STAT_ENEMIES_KILLED", currentEnemyKills);

            SteamUserStats.SetAchievement("ACH_KILL_ENEMY");

            Steamworks.SteamUserStats.StoreStats();
        }

        if (isDead)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void Alive()
    {
        if (!isDead)
        {
            transform.parent.gameObject.SetActive(true);
        }
    }
}
