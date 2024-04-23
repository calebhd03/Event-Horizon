using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class crabEnemy : MonoBehaviour
{
    [SerializeField] EnemyHealthBar healthBar;
    public Transform player;
    public NavMeshAgent agent;
    private Rigidbody rb;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private ThirdPersonShooterController ThirdPersonShooterController;
    private Animator animator;
    public LayerMask playerZone;
    private bool knifeDeath = false;

    //check to find player
    private bool iSeeYou;
    public float seeDistance;

    [Header("Attack")]
    public float attackRange;
    private bool withInAttackRange;
    public float jumpHeight = 1f;
    public float jumpSpeed = 1f;
    private bool jump = false;
    private bool stuck = false;
    public float landingOffset = .5f;
    
    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;

    [Header("Audio")]
    AudioSource audioSource;
    public AudioClip deathAudio;
    public AudioClip stickAudio;
    HealthMetrics healthMetrics;

    private bool isDead = false;//assuming it is alive
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
        ThirdPersonShooterController = FindAnyObjectByType<ThirdPersonShooterController>();
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
        KnifeDestroy();
        updateHealth();
        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        withInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerZone);

        if (iSeeYou == true && withInAttackRange == false)
        {
            transform.LookAt(player);
            ChasePlayer();
        }

        if (iSeeYou == true && withInAttackRange == true)
        {
            AttackPlayer();
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        if(healthMetrics.currentHealth <= 0)
        {
            iSeeYou = false;
        }
    }
    
    private void ChasePlayer() //chase player once found
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        jump= false;
    }

    private void AttackPlayer()
    {
        if (!jump)
        {
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        jump = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = player.position + player.forward * landingOffset;

        float elapsedTime = 0f;

        while (elapsedTime < jumpSpeed)
        {
            float jumpProgress = elapsedTime / jumpSpeed;
            float jumpHeightOffset = Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;

            transform.position = Vector3.Lerp(startPosition, targetPosition, jumpProgress) + new Vector3(0f, jumpHeightOffset, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        jump = false;
    }
    public void updateHealth()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            isDead = true;
            jump = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !stuck)
        {
            Debug.Log("Stuck to Player");
            stuck = true;
            iSeeYou = false;
            attackRange = 0f;
            seeDistance = 0f;
            jump = true;
            StartCoroutine(StickyDelay());

            if(thirdPersonController != null)
            {
                thirdPersonController.MoveSpeed = 1.5f;
                thirdPersonController.SprintSpeed = 1.5f;
                Debug.Log("Speed has been changed");

            }
            else
            {
                Debug.Log("Move Speed can not be found");
            }
        }
    }
    private IEnumerator StickyDelay()
    {
        yield return new WaitForSeconds(0.5f); 
        agent.enabled = false;
        Vector3 offset = new Vector3(0f, 0.5f, .3f);
        transform.parent = player;
        transform.position = player.position;
        transform.localPosition = offset;
        audioSource.PlayOneShot(stickAudio);
    }

    public void KnifeDestroy()
    {
        if(stuck)
        {
            if(ThirdPersonShooterController.knifeSlash == true)
            {
                knifeDeath = true;
                if(knifeDeath)
                {
                    Debug.Log("Crab is Destroyed with Knife");
                    isDead = true;
                    Dead();
                    audioSource.PlayOneShot(deathAudio);
                    if (thirdPersonController != null)
                    {
                        thirdPersonController.MoveSpeed = 3f;
                        thirdPersonController.SprintSpeed = 6f;
                        Debug.Log("Speed has been changed back");
                    }
                }
            }
        }
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
        int currentEnemyKills;
        Steamworks.SteamUserStats.GetStat("STAT_ENEMIES_KILLED", out currentEnemyKills);
        currentEnemyKills++;
        Steamworks.SteamUserStats.SetStat("STAT_ENEMIES_KILLED", currentEnemyKills);

        SteamUserStats.SetAchievement("ACH_KILL_ENEMY");

        Steamworks.SteamUserStats.StoreStats();
        
        if (isDead)
        {
            thirdPersonController.MoveSpeed = 3f;
            thirdPersonController.SprintSpeed = 6f;
            gameObject.SetActive(false);
            transform.parent = null;
        }
    }

    public void Alive()
    {
        if (!isDead)
        {
            gameObject.SetActive(true);
        }
    }
}
