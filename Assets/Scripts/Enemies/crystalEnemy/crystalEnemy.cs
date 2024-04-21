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
    private bool iHearYou;
    public float hearDistance;

    [Header("Attack")]
    public float attackRange;
    private bool withInAttackRange;
    private bool isAttacking = false;
    public float attackAnimationDuration = 3.0f;
    //public float moveBackDistance = 3.0f;
    public float attackCooldown = 6.0f;
    private float nextAttackTime = 0.0f;
    private bool isMovingBackwards;
    private float backwardSpeed = 5.0f;
    private float backWardMoveDuration = 2.0f;

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
    AudioSource audioSource;
    public AudioClip rangedAudio;
    public AudioClip hitAudio;
    public AudioClip deathAudio;
    HealthMetrics healthMetrics;

    private bool isDead = false;//assuming it is alive

    public bool isPhaseTwo = false; //only for the singularity phase two fight
    public GameObject orbPrefab;

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
        healthMetrics = GetComponentInParent<HealthMetrics>();
        healthMetrics.currentHealth = healthMetrics.maxHealth;
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(EnemyMusic());
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
                if(healthMetrics.currentHealth > 0)
                    {
                    iSeeYou = true;
                    }
                transform.LookAt(player);
                audioSource.PlayOneShot(rangedAudio);
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

        iHearYou = Physics.CheckSphere(transform.position, hearDistance, playerZone);
        withInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerZone);

        if (iHearYou == true)
        {
            audioSource.PlayOneShot(rangedAudio);
            iSeeYou = true;
        }

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
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void attackPlayer()
    {
        if (isAttacking == false && Time.time >= nextAttackTime)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            isAttacking = true;

            nextAttackTime = Time.time + attackCooldown;

            if (isAttacking == true)
            {
                animator.SetBool("MeleeAttack", true);
                audioSource.PlayOneShot(rangedAudio);
            }

            else
            {
                animator.SetBool("MeleeAttack", false);
            }

            Invoke(nameof(meleeAttackCoolDown), attackAnimationDuration);
        }
    }

    private void meleeAttackCoolDown()
    {
        //animator.SetBool("MeleeAttack", false);
        isAttacking = false;
        if (!isMovingBackwards)
        {
            isMovingBackwards = true;
            StartCoroutine(moveBackWards());
        }
        else
        {
            StopCoroutine(moveBackWards());
        }
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        //Debug.Log("Sword Recharge");
    }

    private IEnumerator moveBackWards()
    {
        float Timer = 0f;
        while (Timer < backWardMoveDuration)
        {
            float backStep = backwardSpeed * Time.deltaTime;
            Vector3 backwardDirection = -agent.transform.forward * backStep;
            agent.Move(backwardDirection);
            Timer += Time.deltaTime;
            yield return null;
        }

        isMovingBackwards = false;
    }

    /*private void MoveBackAfterAttack()
    {
        Debug.Log("Moving back");
        isAttacking = false;

        Vector3 toPlayerDirection = (player.position - transform.position).normalized;
        Vector3 moveBackPosition = transform.position - toPlayerDirection * moveBackDistance;
        agent.SetDestination(moveBackPosition);
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }*/

    public void updateHealth()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
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
        StartCoroutine(WaitAndDropStuff(3f));
        iSeeYou = false;
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
        if (isPhaseTwo)
        {
            Instantiate(orbPrefab, transform.position, Quaternion.identity);
        }

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

        Dead();
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
        Gizmos.DrawWireSphere(transform.position, hearDistance);
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
