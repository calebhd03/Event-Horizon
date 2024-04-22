using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Steamworks;

public class dogEnemy : MonoBehaviour
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

    [Header("Patrol")]
    public Transform[] movePoints;
    private int destinationPoints = 0;

    [Header("Attack")]
    public float attackRange;
    private bool withInAttackRange;
    private bool isAttacking = false;
    public float attackCloseDistance = 2.5f;
    public float attackAnimationDuration = 2.0f;
    public float moveBackDistance = 3.0f;
    private float attackCooldown = 15.0f;
    private float nextAttackTime = 0.0f;
    public float moveDistance = 3f;
    private float animationEndDelay = 6.0f; // must be longer did animation duration float above in the header attack
    private float lastMoveTime;

    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    [Header("Audio")]
    AudioSource audioSource;
    //public AudioClip rangedAudio;
    public AudioClip deathAudio;
    //public AudioClip hitAudio;
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
                //audioSource.PlayOneShot(rangedAudio);
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
        iHearYou = Physics.CheckSphere(transform.position, hearDistance, playerZone);
        if (iHearYou == true)
        {
            //audioSource.PlayOneShot(rangedAudio);
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
            //audioSource.PlayOneShot(rangedAudio);
        }

        if (iSeeYou == true && withInAttackRange == true)
        {
            //animator.SetBool("Attack", true);
            attackPlayer();
            //audioSource.PlayOneShot(rangedAudio);
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
        nextAttackTime = 0f;
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);
        animator.SetBool("Walk", false);
        animator.SetBool("LJump", false);
        animator.SetBool("RJump", false);
        animator.SetBool("Idle", false);
        agent.SetDestination(player.position);
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void attackPlayer()
    {
        if (!isAttacking && Time.time >= nextAttackTime)
        {
            Debug.Log("Attack Dog");
            animator.SetBool("Attack", true);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("LJump", false);
            animator.SetBool("RJump", false);
            isAttacking = true;

            // Set the next allowed attack time based on the cooldown
            nextAttackTime = Time.time + attackCooldown;

            Vector3 attackPosition = player.position - transform.forward * attackCloseDistance;
            agent.SetDestination(attackPosition);
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Invoke(nameof(MoveBackAfterAttack), attackAnimationDuration);
            InvokeRepeating("AttackMoving", animationEndDelay, 1f);
            Invoke(nameof(CancelAttackMoving), attackCooldown - .1f);
            //audioSource.PlayOneShot(hitAudio);
        }
    }

    private void MoveBackAfterAttack()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("LJump", false);
        animator.SetBool("RJump", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        Debug.Log("Moving back");
        isAttacking = false;

        Vector3 toPlayerDirection = (player.position - transform.position).normalized;
        Vector3 moveBackPosition = transform.position - toPlayerDirection * moveBackDistance;
        agent.SetDestination(moveBackPosition);
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }


    private void AttackMoving()
    {

        ResetTriggers();
        if (Time.time - lastMoveTime >= 1f)
        {
            Debug.Log("Moving left and right");
            Vector3 rightDestination = agent.transform.position + transform.right * moveDistance;
            Vector3 leftDestination = agent.transform.position - transform.right * moveDistance;

            if (Random.value > 0.5f)
            {
                
                animator.SetBool("LJump", true);
                animator.SetBool("RJump", false);
                animator.SetBool("Idle", false);
                animator.SetBool("walk", false);
                animator.SetBool("Attack", false);
                animator.SetBool("Run", false);
                //agent.SetDestination(leftDestination);
            }
            else
            {
                animator.SetBool("RJump", true);
                animator.SetBool("LJump", false);
                animator.SetBool("Idle", false);
                animator.SetBool("walk", false);
                animator.SetBool("Attack", false);
                animator.SetBool("Run", false);
                //agent.SetDestination(rightDestination);
            }

            // Update the time of the last move
            lastMoveTime = Time.time;
        }
        ResetTriggers();
    }

    private void ResetTriggers()
    {
        animator.SetBool("RJumper", false);
        animator.SetBool("LJumper", false);
    }

    private void CancelAttackMoving()
    {
        Debug.Log("Cancel left and right");
        CancelInvoke("AttackMoving");
    }
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
        int currentDogKills;
        Steamworks.SteamUserStats.GetStat("STAT_DOG_KILLS", out currentDogKills);
        currentDogKills++;
        Steamworks.SteamUserStats.SetStat("STAT_DOG_KILLS", currentDogKills);
        Steamworks.SteamUserStats.StoreStats();

        agent.isStopped = true;
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
        if(isPhaseTwo)
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
