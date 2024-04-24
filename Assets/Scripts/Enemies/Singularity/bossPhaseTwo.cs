 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bossPhaseTwo : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public LayerMask playerZone;
    [SerializeField] EnemyHealthBar healthBar;
    private Rigidbody rb;
    [SerializeField] private UpgradeEffects upgrades;
    [SerializeField] private HealthMetrics health;
    [SerializeField] private regularPoint regular;
    [SerializeField] private weakPoint weak;

    private bool iSeeYou;
    public float seeDistance;
    public float allAttackCoolDown;

    [Header("Meteor Attack")]
    public GameObject meteorPortalPrefab;
    public GameObject meteorPrefab;
    public Transform rightMeteor;
    public Transform leftMeteor;
    public Transform middleMeteor;
    public float meteorWindUp;
    public float timeBetweenMeteorAttack;
    public float meteorWaitOnSpawn;
    public float meteorSpeed = 10f;
    
    [Header("Audio")]
    AudioSource audioSource1;
    public AudioClip meteorSpawnSound;
    public AudioClip deathAudio;

    [Header("Enemy Spawns")]
    public float summonWindUp;
    public GameObject[] Enemies;
    public float maxEnemySpawnDistance = 6f;
    public LayerMask navMeshLayer;

    [Header("AOE Attack")]
    public GameObject aoeRingPrefab;
    public GameObject aoeWarningPrefab;
    public Transform aoeSpawn;
    public float aoeWindUp = 2f;
    

    [Header("Drops")]
    public GameObject blasterPickupPrefab;
    public GameObject shotGunPickupPrefab;
    public GameObject bHPickupPrefab;
    public GameObject healthPickupPrefab;
    public float pickupDropChance = 0.3f;
    public GameObject Portal;

    //attck bools
    private bool meteorBool = false;
    private bool enemyBool = false;
    private bool aoeBool = false;

    public bool isDead = false;//assuming it is alive

    //capture
    public bool captured = false;

    //private float timer;
    public static bool shootingEnding = false;
    public static bool captureEnding = false;

    public static bool noBulletDamage = false;

    private int lastAttack = -1;
    private bool lookCheck = true;
    public float timer = 0;

    public GameObject purpleWall;

    PlayerHealthMetric playerHealthMetric;
    GameObject playerr;

    public GameObject SingularityTransition;

    public GameObject bossPhaseGameObject;
    public bossEnemy boss1Script;
    public HealthMetrics boss1health;
    public Transform spawnOrginal;

    [SerializeField] GameObject[] ObstaclesToHide;
    [SerializeField] GameObject[] ObstaclesToNOTHide;




    private void OnEnable()
    {
        noBulletDamage = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource1 = GetComponent<AudioSource>();
        health = GetComponentInParent<HealthMetrics>();
        upgrades = GetComponent<UpgradeEffects>();
        upgrades.knockBackUp = false;
        weak = GetComponentInChildren<weakPoint>();
        regular = GetComponentInChildren<regularPoint>();

        playerr = GameObject.FindWithTag("Player");
        playerHealthMetric = player.GetComponent<PlayerHealthMetric>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
        weak.SingularityDamage();
        regular.SingularityDamage();
        SceneManagement();
        // Increase timer by Time.deltaTime each frame
        //timer += Time.deltaTime;

        // Output the timer value to the console for debugging
        //Debug.Log("Attack1 Timer: " + timer.ToString("F2")); // "F2" formats the timer value to 2 decimal places

        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        updateHealth();
        resetTriggers();
        if (iSeeYou)
        {
            if(lookCheck)
            {
                transform.LookAt(player);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }

            if (!enemyBool && !aoeBool && !meteorBool && health.currentHealth > 0)
            {
                RandomAttack();
            }
        }
    }

    private void RandomAttack()
    {
        int randomAttack;
        do
        {
            randomAttack = Random.Range(0, 4); // 0: SummonEnemies, 1: AOE, 2: Meteor
        } while (randomAttack == lastAttack);

        lastAttack = randomAttack;

        switch (randomAttack)
        {
            case 0:
                //Debug.Log("Attack1 Sumon");
                enemyBool = true;
                StartCoroutine(summonEnemies());
                StopCoroutine(AOE());
                StopCoroutine(PerformMeteor());
                break;
            case 1:
                //Debug.Log("Attack1 AOE");
                aoeBool = true;
                StartCoroutine(AOE());
                StopCoroutine(summonEnemies());
                StopCoroutine(PerformMeteor());
                break;
            case 2:
                //Debug.Log("Attack1 Meteor");
                meteorBool = true;
                StartCoroutine(PerformMeteor());
                StopCoroutine(summonEnemies());
                StopCoroutine(AOE());
                break;
            case 3:
                //Debug.Log("Attack1 Sumon");
                enemyBool = true;
                StartCoroutine(summonEnemies());
                StopCoroutine(AOE());
                StopCoroutine(PerformMeteor());
                break;
        }

        StartCoroutine(attackCoolDown());
    }

    private IEnumerator attackCoolDown()
    {
        yield return new WaitForSeconds(allAttackCoolDown);
        //reset bools to false after 10 seconds for atttack cooldown
        ResetBools();
    }

    private void ResetBools()
    {
        meteorBool = false;
        enemyBool = false;
        aoeBool = false;
    }
    private IEnumerator summonEnemies()
    {
        animator.SetBool("P2Attack2", true);
        yield return new WaitForSeconds(summonWindUp);

        Vector3 spawnOffset = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        Vector3 spawnPosition = player.position + spawnOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, maxEnemySpawnDistance, navMeshLayer))
        {
            spawnPosition = hit.position;

            if (Vector3.Distance(spawnPosition, player.position) <= maxEnemySpawnDistance)
            {
                foreach (GameObject enemy in Enemies)
                {
                    Instantiate(enemy, spawnPosition, Quaternion.identity);
                }
            }
            else
            {
                Debug.LogWarning("Spawn position is too far from the player.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot find a valid spawn position on the NavMesh.");
        }
        animator.SetBool("P2Attack2", false);
    }

    private IEnumerator AOE()
    {
        lookCheck = false;
        animator.SetBool("P2Attack1", true);
        //set animator
        // animator.SetTrigger("AOEAttack");
        GameObject newWarningRingAOE = Instantiate(aoeWarningPrefab, player.position, Quaternion.identity);
        transform.LookAt(newWarningRingAOE.transform.position);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        yield return new WaitForSeconds(8.5f);
        lookCheck = true;
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Debug.Log("Animation Fist Attack");
        yield return new WaitForSeconds(.5f);

        GameObject newRingAOE = Instantiate(aoeRingPrefab, newWarningRingAOE.transform.position, Quaternion.identity);
        Destroy(newRingAOE, 5f);
        Destroy(newWarningRingAOE, 5f);
        animator.SetBool("P2Attack1", false);

    }

    private IEnumerator PerformMeteor()
    {
        animator.SetBool("P2Attack3", true);
        summonMeteorPortal(rightMeteor.position, Quaternion.identity);
        summonMeteorPortal(leftMeteor.position, Quaternion.identity);
        summonMeteorPortal(middleMeteor.position, Quaternion.identity);

        yield return new WaitForSeconds(meteorWindUp);
        MeteorSpawnSound();
        summonMeteor(rightMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        MeteorSpawnSound();
        summonMeteor(leftMeteor.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenMeteorAttack);

        MeteorSpawnSound();
        summonMeteor(middleMeteor.position, Quaternion.identity);
        animator.SetBool("P2Attack3", false);
    }

    private void MeteorSpawnSound()
    {
        if (player != null && meteorSpawnSound != null)
        {
            AudioSource audioSource = player.GetComponentInChildren<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(meteorSpawnSound);
            }
        }
    }

    private void summonMeteor(Vector3 position, Quaternion rotation)
    {
        Rigidbody newMeteor = Instantiate(meteorPrefab, position, rotation).GetComponent<Rigidbody>();
        Vector3 directionToPlayer = player.position - position;

        float yOffset = 2.0f;
        directionToPlayer.y += yOffset;

        directionToPlayer.Normalize();

        StartCoroutine(MeteorDelay(newMeteor, directionToPlayer));
    }

    private IEnumerator MeteorDelay(Rigidbody meteorRigidbody, Vector3 directionToPlayer)
    {
        yield return new WaitForSeconds(meteorWaitOnSpawn);
        meteorRigidbody.velocity = directionToPlayer * meteorSpeed;
        Destroy(meteorRigidbody.gameObject, 10f);
    }

    public void summonMeteorPortal(Vector3 position, Quaternion rotation)
    {
        GameObject newMeteorPortal = Instantiate(meteorPortalPrefab, position, rotation);
        Destroy(newMeteorPortal.gameObject, 8f);
    }

    public void updateHealth()
    {
        //HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(health.currentHealth, health.maxHealth);

        if (health.currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        if(captured)
        {
            isDead = true;
        }
        animator.SetBool("P2Attack1", false);
        animator.SetBool("P2Attack2", false);
        animator.SetBool("P2Attack3", false);
        animator.SetBool("Death", true);
        Debug.Log("Die Function");
        //Debug.Log("Boss Death starting");
        StartCoroutine(WaitAndDropStuff(4f));
    }

    private IEnumerator WaitAndDropStuff(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        audioSource1.PlayOneShot(deathAudio);
        if (Background_Music.instance != null)
            Background_Music.instance.CenterMusic();

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


        Portal.SetActive(true);
        //Debug.Log("Boss Death end");
        //Destroy(transform.parent.gameObject);
        Dead();
        DestroySummons();
    }

    private void resetTriggers()
    {
    }

    public void PlayEnemyHitAnimation()
    {
        //Trigger the "EnemyHit" animation
        animator.SetTrigger("EnemyHit");
    }

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

    public void DestroySummons()
    {
        GameObject[] summonedEnemies = GameObject.FindGameObjectsWithTag("SummonEnemy");
        foreach (GameObject enemy in summonedEnemies)
        {
            Destroy(enemy);
        }

        GameObject[] Orbs = GameObject.FindGameObjectsWithTag("Orb");
        foreach (GameObject Orb in Orbs)
        {
            Destroy(Orb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet") || other.CompareTag("Plasma Bullet") || other.CompareTag("BHBullet"))
        {
            Vector3 spawnOffset = new Vector3 (0, 0, -1f);
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);

            Vector3 spawnPosition = collisionPoint + spawnOffset;

            GameObject purple = Instantiate(purpleWall, spawnPosition, Quaternion.identity);
            purple.transform.LookAt(player);
            purple.transform.rotation = Quaternion.Euler(0f, purple.transform.rotation.eulerAngles.y, purple.transform.rotation.eulerAngles.z);
            Destroy(purple, 1f);
        }
    }

    public void SceneManagement()
    {
        if(OrbFunction.orbCount >= 4)
        {
            captured = true;
        }

        if(captured)
        {
            Die();
            captureEnding = true;
            shootingEnding = false;
        }

        else
        {
            shootingEnding = true;
            captureEnding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }

    public void CheckPlayer()
    {
        if (playerHealthMetric.playerData.currentHealth <= 0)
        {
            DestroySummons();
            health.currentHealth = health.maxHealth;
            Vector3 ResetLoaction = new Vector3(-3.06f, -38.7f, -0.78f);
            ResetLocationForPhase2(ResetLoaction);
            boss1Script.isDead = false;
            boss1health.currentHealth = boss1health.maxHealth;
            bossPhaseGameObject.SetActive(true);
            bossPhaseGameObject.transform.parent.gameObject.SetActive(true);
            ResetLocationForPhase1();
            StartCoroutine(DelayToReset());
            OrbFunction.orbCount = 0;

            foreach (GameObject obj in ObstaclesToHide)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in ObstaclesToNOTHide)
            {
                obj.SetActive(true);
            }


        }
    }

    IEnumerator DelayToReset()
    {
        yield return new WaitForSeconds(1f);
        transform.parent.gameObject.SetActive(false);
        SingularityTransition.SetActive(false);
    }

    public void ResetLocationForPhase2(Vector3 reset)
    {
        transform.position = reset;
    }

    public void ResetLocationForPhase1()
    {
        bossPhaseGameObject.transform.position = spawnOrginal.position;
    }
}