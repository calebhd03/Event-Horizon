using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class bossEnemy : MonoBehaviour
{
    //get variables
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    [SerializeField] private UpgradeEffects upgrades;

    //layerCheck
    public LayerMask playerZone;

    //See
    private bool iSeeYou;
    public float seeDistance;

    //IfTooCloseStop
    private bool stopDistance;
    public float stopDistanceRange = 3.0f;

    //Meteor Attack
    public GameObject meteorPortalPrefab;
    public GameObject meteorPrefab;
    public Transform rightMeteor;
    public Transform leftMeteor;
    public Transform middleMeteor;
    public float meteorWindUp;
    public float timeBetweenMeteorAttack;
    public float meteorWaitOnSpawn;
    public float meteorSpeed = 10f;
    private bool meteorAttack = false;
    private float meteorAttackCooldown = 10.0f;
    private float timeSinceLastMeteorAttack;

    //MeleeAttack
    public float slashWindUp = 12f;
    private bool slashAttack = false;
    private float slashAttackCooldown = 10.0f;
    private float timeSinceLastSlashAttack;
    private Animator armAnim;

    //AOEAttack
    public GameObject aoeRingPrefab; //test object
    public GameObject aoeCloseRingPrefab;
    public GameObject aoeCloseWarningPrefab;
    public Transform aoeSpawn;
    public float aoeWindUp = 2f;
    private bool aoeAttack = false;
    private float aoeAttackCooldown = 10.0f;
    private float timeSinceLastAOEAttack;

    [SerializeField] EnemyHealthBar healthBar;
    private Rigidbody rb;

    [Header("Audio")]
    AudioSource audioSource1;
    public AudioClip meteorSpawnSound;
    public AudioClip deathAudio;

    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;
    public GameObject Portal;
    public GameObject deathParticle;
    public Vector3 deathParticleOffset;
    public float timeTillDestroyOnDeath;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInParent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        rb = GetComponent<Rigidbody>();
        upgrades = GetComponent<UpgradeEffects>();
        //Portal.SetActive(false);

        Transform childTransform = transform.Find("rightArmSlash");
        if (childTransform != null)
        {
            armAnim = childTransform.GetComponent<Animator>();
        }
        audioSource1 = GetComponent<AudioSource>();
        StartCoroutine(BossMusic());
        upgrades.knockBackUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        // BE CAREFUL: putting code below this will not be run when boss is dead
        if (isDead) return;


        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        stopDistance = Physics.CheckSphere(transform.position, stopDistanceRange, playerZone);

        

        updateHealth();
        resetTriggers();
        updateSpeed();

        if (iSeeYou && !meteorAttack && Time.time - timeSinceLastMeteorAttack > meteorAttackCooldown && !stopDistance)
        {
            float randomValue = Random.value;

            transform.LookAt(player);

            if (randomValue < 0.5f)
            {
                StartCoroutine(PerformMeteor());
                StopCoroutine(AOE());
                aoeAttack = true;
            }
            else
            {
                aoeAttack = false;
            }
        }

        if (iSeeYou && !aoeAttack && Time.time - timeSinceLastAOEAttack > aoeAttackCooldown)
        {
            float randomValue = Random.value;

            if (randomValue < 0.5f)
            {
                StartCoroutine(AOE());
                StopCoroutine(PerformMeteor());
                meteorAttack = true;
            }
            else
            {
                meteorAttack = false;
            }
        }

        if (iSeeYou)
        {
            followPlayer();
            if (stopDistance == true)
            {
                //stops the metoer attack when player is close to the enemy
                //meteorAttack = true;

                agent.SetDestination(transform.position);
                transform.LookAt(player);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                if (!aoeAttack && Time.time - timeSinceLastAOEAttack > aoeAttackCooldown)
                {
                    float randomValue = Random.value;

                    if (randomValue < 0.5f)
                    {
                        StartCoroutine(AOE());

                        //This will make sure the AOE attack works and does not happen at the same time as the slash attack
                        StopCoroutine(slash());
                        slashAttack = true;
                        //armAnim.SetBool("Slash180", false);
                    }

                    //if the attack is not a aoe at random then slash attack bool is false which where then trigger the slash attack
                    else
                    {
                        slashAttack = false;
                    }
                }

                // Check for slash attack
                if (!slashAttack && Time.time - timeSinceLastSlashAttack > slashAttackCooldown)
                {
                    float randomValue = Random.value;

                    if (randomValue < 0.5f)
                    {
                        StartCoroutine(slash());

                        //This will make sure the Slash attack works and does not happen at the same time as the AOE attack
                        StopCoroutine(AOE());
                        aoeAttack = true;
                        //armAnim.SetBool("Slash180", false);
                    }

                    //if the attack is not a slash at random then aie attack bool is false which where then trigger the aoe attack
                    else
                    {
                        aoeAttack = false;
                    }
                }

            }

            if(!stopDistance)
            {
                //armAnim.SetBool("Slash180", false);
            }
        }
    }

    private void resetTriggers()
    {
        animator.ResetTrigger("EnemyHit");
        animator.ResetTrigger("SlashAttack");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("AOEAttack");
    }

    private void updateSpeed()
    {
        //Debug.Log("agent.velocity = " + agent.velocity);
        //Debug.Log("agent.isStopped = " + agent.isStopped);
        animator.SetInteger("MovSpeed", (int) agent.velocity.magnitude);
    }

    public void followPlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }

    IEnumerator PerformMeteor()
    {
        animator.SetBool("P1Attack1", true);
        agent.isStopped = true;
        meteorAttack = true;
        summonMeteorPortal(rightMeteor.position, Quaternion.identity);
        summonMeteorPortal(leftMeteor.position, Quaternion.identity);
        summonMeteorPortal(middleMeteor.position, Quaternion.identity);

        yield return new WaitForSeconds(meteorWindUp);

        if (player != null && meteorSpawnSound != null)
        {
            AudioSource audioSource = player.GetComponentInChildren<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(meteorSpawnSound);
            }
        }
        summonMeteor(rightMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        if (player != null && meteorSpawnSound != null)
        {
            AudioSource audioSource = player.GetComponentInChildren<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(meteorSpawnSound);
            }
        }
        summonMeteor(leftMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        if (player != null && meteorSpawnSound != null)
        {
            AudioSource audioSource = player.GetComponentInChildren<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(meteorSpawnSound);
            }
        }
        summonMeteor(middleMeteor.position, Quaternion.identity);

        meteorAttack = false;
        agent.isStopped = false;
        timeSinceLastMeteorAttack = Time.time;
        animator.SetBool("P1Attack1", false);
    }

    public void summonMeteor(Vector3 position, Quaternion rotation)
    {
        Rigidbody newMeteor = Instantiate(meteorPrefab, position, rotation).GetComponent<Rigidbody>();
        Vector3 directionToPlayer = player.position - position;

        float yOffset = 2.0f;
        directionToPlayer.y += yOffset;

        directionToPlayer.Normalize();

        StartCoroutine(MeteorDelay(newMeteor, directionToPlayer));
    }

    IEnumerator MeteorDelay(Rigidbody meteorRigidbody, Vector3 directionToPlayer)
    {
        yield return new WaitForSeconds(meteorWaitOnSpawn);
        meteorRigidbody.velocity = directionToPlayer * meteorSpeed;
        Destroy(meteorRigidbody.gameObject, 10f);
    }



    public void summonMeteorPortal (Vector3 position, Quaternion rotation)
    {
        GameObject newMeteorPortal = Instantiate(meteorPortalPrefab, position, rotation);
        Destroy(newMeteorPortal.gameObject, 8f);
    }

    IEnumerator slash()
    {
        animator.SetBool("P1Attack3", true);
        agent.isStopped = true;
        slashAttack = true;

        yield return new WaitForSeconds(slashWindUp);
        
        //armAnim.SetBool("Slash180", true);
        animator.SetTrigger("SlashAttack");

        slashAttack = false;
        agent.isStopped = false;
        timeSinceLastSlashAttack = Time.time;
        animator.SetBool("P1Attack3", false);
        Debug.Log("Slash Attack from boss");
    }

    IEnumerator AOE()
    {
        animator.SetBool("P1Attack2", true);
        agent.isStopped = true;
        aoeAttack = true;
        
        //set animator
        animator.SetTrigger("AOEAttack");
        GameObject newWarningRingAOE = Instantiate(aoeCloseWarningPrefab, aoeSpawn.position, Quaternion.identity);

        yield return new WaitForSeconds(aoeWindUp);
        Destroy(newWarningRingAOE);

        GameObject newRingAOE = Instantiate(aoeRingPrefab, aoeSpawn.position, Quaternion.identity);
        GameObject newCloseRingAoe = Instantiate(aoeCloseRingPrefab, aoeSpawn.position, Quaternion.identity);
        Destroy(newRingAOE, 5f);
        Destroy(newCloseRingAoe, 5f);

        aoeAttack = false;
        agent.isStopped = false;
        timeSinceLastAOEAttack = Time.time;
        animator.SetBool("P1Attack2", false);
    }

    public void updateHealth()
    {
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if(healthMetrics.currentHealth <= 0)
        {
            Die();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, stopDistanceRange);
    }

     public void Die()
        {
            // Stop the NavMeshAgent to prevent further movement
            agent.isStopped = true;
            Debug.Log("Boss Death starting");
            animator.SetBool("Death", true);


            // Wait for 3 seconds before dropping stuff
            StartCoroutine(WaitAndDropStuff(4f));
        }

        private IEnumerator WaitAndDropStuff(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            audioSource1.PlayOneShot(deathAudio);

            // Call DropStuff after waiting for 3 seconds
            DropStuff();
        }

        private void DropStuff()
        {
            if (isDead) return;

            isDead = true;

            if(Random.value < pickupDropChance)
            {
                Instantiate(shotGunPickupPrefab, transform.position, Quaternion.identity);
                Instantiate(blasterPickupPrefab, transform.position, Quaternion.identity);
                Instantiate(bHPickupPrefab, transform.position, Quaternion.identity);
            }

            if (Random.value < pickupDropChance / 2)
            {
                Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
            }
           
            
            Portal.SetActive(true);
            Instantiate(deathParticle, transform.position + deathParticleOffset, Quaternion.identity);
            Debug.Log("Boss Death end");
            Destroy(transform.parent.gameObject, timeTillDestroyOnDeath);
        }
         public void PlayEnemyHitAnimation()
        {
            //Trigger the "EnemyHit" animation
            animator.SetTrigger("EnemyHit");
        }

        IEnumerator BossMusic()
        {
            yield return new WaitUntil(() => iSeeYou);
            Background_Music.instance.BossMusic();
            yield return null;
            
        }
        
}
