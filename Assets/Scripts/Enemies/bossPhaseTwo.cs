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

    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource1 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer by Time.deltaTime each frame
        timer += Time.deltaTime;

        // Output the timer value to the console for debugging
        Debug.Log("Timer: " + timer.ToString("F2")); // "F2" formats the timer value to 2 decimal places

        iSeeYou = Physics.CheckSphere(transform.position, seeDistance, playerZone);
        updateHealth();
        resetTriggers();
        if (iSeeYou)
        {
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            if(!enemyBool && !aoeBool && !meteorBool)
            {
                RandomAttack();
            }
        }
    }

    private void RandomAttack()
    {
        int randomAttack = Random.Range(0, 3); // 0: SummonEnemies, 1: AOE, 2: Meteor

        switch (randomAttack)
        {
            case 0:
                enemyBool = true;
                StartCoroutine(summonEnemies());
                StopCoroutine(AOE());
                StopCoroutine(PerformMeteor());
                break;
            case 1:
                aoeBool = true;
                StartCoroutine(AOE());
                StopCoroutine(summonEnemies());
                StopCoroutine(PerformMeteor());
                break;
            case 2:
                meteorBool = true;
                StartCoroutine(PerformMeteor());
                StopCoroutine(summonEnemies());
                StopCoroutine(AOE());
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
        yield return new WaitForSeconds(summonWindUp);
        foreach (GameObject enemy in Enemies)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

            Vector3 spawnPosition = player.position + spawnOffset;
            Instantiate(enemy, spawnPosition, Quaternion.identity); 
        }
    }

    private IEnumerator AOE()
    {
        //set animator
        // animator.SetTrigger("AOEAttack");
        GameObject newWarningRingAOE = Instantiate(aoeWarningPrefab, player.position, Quaternion.identity);

        yield return new WaitForSeconds(aoeWindUp);
        Debug.Log("Animation Fist Attack");
        yield return new WaitForSeconds(2f);

        GameObject newRingAOE = Instantiate(aoeRingPrefab, newWarningRingAOE.transform.position, Quaternion.identity);
        Destroy(newRingAOE, 5f);
        Destroy(newWarningRingAOE, 5f);
    }

    private IEnumerator PerformMeteor()
    {
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
        HealthMetrics healthMetrics = GetComponentInParent<HealthMetrics>();
        healthBar.updateHealthBar(healthMetrics.currentHealth, healthMetrics.maxHealth);

        if (healthMetrics.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //Debug.Log("Boss Death starting");
        StartCoroutine(WaitAndDropStuff(1f));
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
        Destroy(transform.parent.gameObject);
    }

    private void resetTriggers()
    {
        animator.ResetTrigger("EnemyHit");
        animator.ResetTrigger("SlashAttack");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("AOEAttack");
    }

    public void PlayEnemyHitAnimation()
    {
        //Trigger the "EnemyHit" animation
        animator.SetTrigger("EnemyHit");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, seeDistance);
    }
}